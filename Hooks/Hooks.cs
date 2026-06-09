using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using SpecFlowDemo.Utilities;
using TechTalk.SpecFlow;

namespace SpecFlowDemo.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly ScenarioContext _scenarioContext;
        private static int _scenarioCount = 0;
        private static int _totalScenarios = 3; // Songs count

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            ReportManager.Initialize();
            Console.WriteLine("[HOOK] Test Run Started");
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            ScreenRecorder.StartRecording(
                _scenarioContext.ScenarioInfo.Title);
            Console.WriteLine(
                $"[HOOK] Starting: {_scenarioContext.ScenarioInfo.Title}");
        }

        [AfterScenario]
        public void AfterScenario()
        {
            // Stop recording + GIF
            string? videoPath = ScreenRecorder.StopRecording();
            if (videoPath != null)
                ScreenRecorder.ConvertToGif(videoPath);

            if (_scenarioContext.TestError != null)
            {
                string? screenshotPath = null;

                if (_scenarioContext.TryGetValue(
                    "Driver", out IWebDriver? driver) && driver != null)
                {
                    try
                    {
                        var ss = ((ITakesScreenshot)driver)
                            .GetScreenshot();
                        screenshotPath = Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory,
                            "..", "..", "..", "Reports",
                            $"fail_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                        Directory.CreateDirectory(
                            Path.GetDirectoryName(screenshotPath)!);
                        ss.SaveAsFile(screenshotPath);
                        Console.WriteLine(
                            $"[SCREENSHOT] {screenshotPath}");
                    }
                    catch { }
                    try { driver.Quit(); } catch { }
                }

                _scenarioContext.TryGetValue(
                    "Steps",
                    out List<ReportManager.StepLog>? steps);
                _scenarioContext.TryGetValue(
                    "ScenarioStart", out DateTime start);

                steps ??= new List<ReportManager.StepLog>();
                steps.Add(new ReportManager.StepLog(
                    "fail",
                    $"FAILED: {_scenarioContext.TestError.Message}",
                    DateTime.Now.ToString("HH:mm:ss")));

                ReportManager.LogFail(
                    _scenarioContext.ScenarioInfo.Title,
                    _scenarioContext.TestError.Message,
                    steps,
                    start == default ? DateTime.Now : start,
                    DateTime.Now,
                    screenshotPath);
            }

            // ✅ Count scenarios — last one aana email send pannu
            _scenarioCount++;
            Console.WriteLine(
                $"[HOOK] Scenario {_scenarioCount}/{_totalScenarios} done");

            if (_scenarioCount >= _totalScenarios)
            {
                Console.WriteLine(
                    "[HOOK] All scenarios done — sending email...");
                SendEmailAndReport();
            }
        }

        private static void SendEmailAndReport()
        {
            try
            {
                ReportManager.Flush();

                // Wait for file to be released
                System.Threading.Thread.Sleep(2000);

                var results = ReportManager.GetResults();
                int pass    = results.Count(r => r.Status == "PASSED");
                int fail    = results.Count(r => r.Status == "FAILED");

                Console.WriteLine("================================");
                Console.WriteLine($"[EMAIL] Pass:{pass} | Fail:{fail}");
                Console.WriteLine("================================");

                string reportPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "..", "..", "..", "Reports", "TestReport.html");

                Console.WriteLine(
                    $"[EMAIL] Report exists: {File.Exists(reportPath)}");

                EmailReporter.SendReport(reportPath, pass, fail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EMAIL] ERROR: {ex.Message}");
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            Console.WriteLine(
                "[REPORT] Reports/TestReport.html — Complete!");
            Console.WriteLine(
                "[RECORD] Reports/Videos/ — MP4 + GIF saved!");
        }
    }
}
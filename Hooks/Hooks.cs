using System;
using System.Collections.Generic;
using System.IO;
using OpenQA.Selenium;
using SpecFlowDemo.Utilities;
using TechTalk.SpecFlow;

namespace SpecFlowDemo.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly ScenarioContext _scenarioContext;

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            ReportManager.Initialize();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            // Auto start recording
            ScreenRecorder.StartRecording(
                _scenarioContext.ScenarioInfo.Title);

            Console.WriteLine(
                $"[HOOK] Starting: {_scenarioContext.ScenarioInfo.Title}");
        }


        [AfterScenario]
        public void AfterScenario()
        {
            // ✅ Auto stop recording + GIF convert
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
                        var ss = ((ITakesScreenshot)driver).GetScreenshot();
                        screenshotPath = Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory,
                            "..", "..", "..", "Reports",
                            $"fail_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                        Directory.CreateDirectory(
                            Path.GetDirectoryName(screenshotPath)!);
                        ss.SaveAsFile(screenshotPath);
                        Console.WriteLine($"[SCREENSHOT] {screenshotPath}");
                    }
                    catch { }

                    try { driver.Quit(); } catch { }
                }

                _scenarioContext.TryGetValue(
                    "Steps", out List<ReportManager.StepLog>? steps);
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

                ReportManager.Flush();
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            Console.WriteLine("[REPORT] Reports/TestReport.html generated!");
            Console.WriteLine("[RECORD] Reports/Videos/ — MP4 + GIF saved!");
        }
    }
}
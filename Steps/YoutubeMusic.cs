using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SpecFlowDemo.Utilities;
using TechTalk.SpecFlow;

namespace SpecFlowDemo.Steps
{
    [Binding]
    public class YoutubeMusicSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver _driver = null!;
        private IJavaScriptExecutor _js = null!;
        private readonly List<ReportManager.StepLog> _steps = new();
        private DateTime _scenarioStart;

        public YoutubeMusicSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _scenarioStart   = DateTime.Now;
        }

        private void Step(string text, string status = "pass")
        {
            _steps.Add(new ReportManager.StepLog(
                status, text, DateTime.Now.ToString("HH:mm:ss")));
            Console.WriteLine($"[{status.ToUpper()}] {text}");
        }

        [Given(@"the user launches the browser")]
        public void GivenTheUserLaunchesTheBrowser()
        {
            _scenarioStart = DateTime.Now;
            var options = new ChromeOptions();
            options.AddArgument("--disable-blink-features=AutomationControlled");
            options.AddExcludedArgument("enable-automation");
            options.AddArgument("--disable-notifications");
            options.AddUserProfilePreference("credentials_enable_service", false);

            string driverPath = Directory.GetCurrentDirectory();
            var service = ChromeDriverService.CreateDefaultService(driverPath);
            service.HideCommandPromptWindow = true;

            _driver = new ChromeDriver(service, options);
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            _js = (IJavaScriptExecutor)_driver;

            _scenarioContext["Driver"]        = _driver;
            _scenarioContext["Steps"]         = _steps;
            _scenarioContext["ScenarioStart"] = _scenarioStart;

            Step("Browser launched — ChromeDriver started");
        }

        [Given(@"the user navigates to YouTube")]
        public void GivenTheUserNavigatesToYouTube()
        {
            _driver.Navigate().GoToUrl("https://www.youtube.com");
            Step("Navigated to https://www.youtube.com");
        }

        [When(@"the user searches for ""(.*)""")]
        public void WhenTheUserSearchesFor(string songName)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            var searchBox = wait.Until(
                d => d.FindElement(By.Name("search_query")));
            searchBox.Clear();
            searchBox.SendKeys(songName);
            searchBox.SendKeys(Keys.Enter);
            Step($"Searched for: {songName}");
        }

        [When(@"the user opens the official song video")]
        public void WhenTheUserOpensTheOfficialSongVideo()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            var firstVideo = wait.Until(
                d => d.FindElement(By.XPath("(//*[@id='video-title'])[1]")));

            string title = firstVideo.Text;
            firstVideo.Click();
            Thread.Sleep(5000);

            var player = _driver.FindElement(By.Id("movie_player"));
            player.SendKeys("f");
            Thread.Sleep(1000);

            _scenarioContext["OriginalUrl"] = _driver.Url;
            Step($"Opened video: {title}");
            Step("Fullscreen enabled (f key)");
        }

        [Then(@"the video should start playing")]
        public void ThenTheVideoShouldStartPlaying()
        {
            for (int i = 0; i < 30; i++)
            {
                try
                {
                    var skipBtns = _driver.FindElements(
                        By.CssSelector(".ytp-skip-ad-button"));
                    if (skipBtns.Count > 0)
                    {
                        skipBtns[0].Click();
                        Step("Ad detected and skipped");
                        break;
                    }
                }
                catch { }
                Thread.Sleep(1000);
            }

            bool isPlaying = Convert.ToBoolean(
                _js.ExecuteScript(
                    "return document.querySelector('video') !== null && " +
                    "!document.querySelector('video').paused;"));

            Assert.That(isPlaying, Is.True, "Video did not start playing!");
            Step($"Video playing confirmed: {isPlaying}");
        }

        [Then(@"the video should complete playback")]
        public void ThenTheVideoShouldCompletePlayback()
        {
            string originalUrl = _scenarioContext["OriginalUrl"].ToString()!;

            double totalSeconds = Convert.ToDouble(
                _js.ExecuteScript(
                    "return document.querySelector('video')?.duration || 0;"));

            TimeSpan totalDuration = TimeSpan.FromSeconds(totalSeconds);
            Step($"Total duration: {totalDuration:hh\\:mm\\:ss}");

            DateTime startTime      = DateTime.Now;
            int lastPrintedMinute   = -1;

            while (true)
            {
                try
                {
                    if (_driver.Url != originalUrl)
                    {
                        Step("Next song auto-detected — playback complete");
                        break;
                    }

                    double currentSeconds = Convert.ToDouble(
                        _js.ExecuteScript(
                            "return document.querySelector('video')?.currentTime || 0;"));

                    TimeSpan currentTime = TimeSpan.FromSeconds(currentSeconds);
                    TimeSpan remaining   = totalDuration - currentTime;
                    int currentMinute    = (int)(DateTime.Now - startTime).TotalMinutes;

                    if (currentMinute > lastPrintedMinute)
                    {
                        lastPrintedMinute = currentMinute;
                        Step($"Progress → Played: {currentTime:hh\\:mm\\:ss} | " +
                             $"Remaining: {remaining:hh\\:mm\\:ss}", "info");
                    }

                    if (currentSeconds >= totalSeconds - 2)
                    {
                        Step("Song completed successfully ✓");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Step($"Warning: {ex.Message}", "info");
                }
                Thread.Sleep(1000);
            }
        }

        [Then(@"the video should be paused")]
        public void ThenTheVideoShouldBePaused()
        {
            try
            {
                _js.ExecuteScript(
                    "if(document.fullscreenElement) document.exitFullscreen();");
                Thread.Sleep(1000);
                var player = _driver.FindElement(By.Id("movie_player"));
                player.SendKeys("k");
                Thread.Sleep(500);
                Step("Video paused + Fullscreen exited");
            }
            catch (Exception ex)
            {
                Step($"Pause warning: {ex.Message}", "info");
            }
        }

        [Then(@"the browser should be minimized")]
        public void ThenTheBrowserShouldBeMinimized()
        {
            _driver.Manage().Window.Minimize();
            Thread.Sleep(1000);
            Step("Browser minimized");
        }

        [Then(@"the browser should be closed")]
        public void ThenTheBrowserShouldBeClosed()
        {
            Step("Browser closing...");
            DateTime end = DateTime.Now;
            ReportManager.LogPass(
                _scenarioContext.ScenarioInfo.Title,
                _steps,
                _scenarioStart,
                end);
            ReportManager.Flush();

            Console.WriteLine();
            Console.WriteLine("=================================");
            Console.WriteLine("FINAL SUMMARY");
            Console.WriteLine($"Status   : PASSED ✅");
            Console.WriteLine($"End Time : {end}");
            Console.WriteLine("=================================");

            _driver.Quit();
        }
    }
}
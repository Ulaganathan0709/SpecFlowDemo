using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SpecFlowDemo.Utilities;

namespace SpecFlowDemo.Pages
{
    public class YoutubePage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly WebDriverWait _longWait;

        private readonly By _searchBox        = By.Name("search_query");
        private readonly By _firstVideoResult = By.CssSelector("ytd-video-renderer:first-of-type #video-title");
        private readonly By _adSkipButton     = By.CssSelector(".ytp-skip-ad-button, .ytp-ad-skip-button");
        private readonly By _videoPlayer      = By.Id("movie_player");
        private readonly By _currentTimeLabel = By.CssSelector(".ytp-time-current");
        private readonly By _durationLabel    = By.CssSelector(".ytp-time-duration");
        private readonly By _adContainer      = By.CssSelector(".ytp-ad-player-overlay");

        public YoutubePage()
        {
            _driver   = DriverManager.GetDriver();
            _wait     = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            _longWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(60));
        }

        public void OpenYoutube()
        {
            _driver.Navigate().GoToUrl("https://www.youtube.com");
            HandleCookieConsent();
        }

        private void HandleCookieConsent()
        {
            try
            {
                var btn = _driver.FindElement(
                    By.XPath("//button[contains(., 'Accept') or contains(., 'agree')]"));
                btn?.Click();
            }
            catch { }
        }

        public void SearchSong(string songName)
        {
            var searchBox = _wait.Until(ExpectedConditions.ElementToBeClickable(_searchBox));
            searchBox.Clear();
            searchBox.SendKeys(songName);
            searchBox.SendKeys(Keys.Enter);
        }

        public void OpenFirstVideo()
        {
            var firstVideo = _wait.Until(ExpectedConditions.ElementToBeClickable(_firstVideoResult));
            Console.WriteLine($"[INFO] Opening: {firstVideo.Text}");
            firstVideo.Click();
            _wait.Until(ExpectedConditions.ElementExists(_videoPlayer));
            Thread.Sleep(2000);

            // Maximize YouTube player to fullscreen
            var player = _driver.FindElement(_videoPlayer);
            player.SendKeys("f"); // YouTube fullscreen shortcut
            Thread.Sleep(1000);
        }
        public void SkipAd()
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    var skip = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                        .Until(ExpectedConditions.ElementToBeClickable(_adSkipButton));
                    skip.Click();
                    Console.WriteLine($"[INFO] Ad skipped (attempt {i + 1})");
                    Thread.Sleep(1500);
                }
                catch
                {
                    if (!IsElementVisible(_adContainer))
                    {
                        Console.WriteLine("[INFO] No ad detected");
                        break;
                    }
                    Thread.Sleep(2000);
                }
            }
        }

        public bool IsVideoPlaying()
        {
            try
            {
                var js = (IJavaScriptExecutor)_driver;
                var result = js.ExecuteScript(
                    "return document.querySelector('video') !== null && " +
                    "!document.querySelector('video').paused;");
                return result is bool b && b;
            }
            catch { return false; }
        }

        public string GetCurrentTime()
        {
            try { return _driver.FindElement(_currentTimeLabel).Text; }
            catch { return "0:00"; }
        }

        public string GetTotalDuration()
        {
            try { return _driver.FindElement(_durationLabel).Text; }
            catch { return "0:00"; }
        }

            public void WaitForVideoCompletion()
    {
        Console.WriteLine("[INFO] Waiting for video to complete...");
        Thread.Sleep(3000); // Wait for duration to load

        string duration = GetTotalDuration();
        Console.WriteLine($"[INFO] Total Duration: {duration}");
        int totalSeconds = ParseTimeToSeconds(duration);

        var timeout = DateTime.Now.AddSeconds(totalSeconds + 30);
        while (DateTime.Now < timeout)
        {
            string current = GetCurrentTime();
            Console.WriteLine($"[INFO] Progress: {current} / {duration}");

            // Check if ended
            if (IsVideoEnded())
            {
                Console.WriteLine("[INFO] Video completed!");
                return;
            }

            // Check if near end (within 2 seconds)
            int currentSec = ParseTimeToSeconds(current);
            if (totalSeconds > 0 && currentSec >= totalSeconds - 2)
            {
                Console.WriteLine("[INFO] Video nearly complete!");
                Thread.Sleep(3000);
                return;
            }

            Thread.Sleep(3000);
        }
        Console.WriteLine("[WARN] Timeout reached");
    }

        private bool IsVideoEnded()
        {
            try
            {
                var js = (IJavaScriptExecutor)_driver;
                var result = js.ExecuteScript(
                    "var v = document.querySelector('video'); return v !== null && v.ended;");
                return result is bool b && b;
            }
            catch { return false; }
        }

        public void PauseVideo()
        {
            try
            {
                // Exit fullscreen first
                var player = _driver.FindElement(_videoPlayer);
                player.SendKeys("f"); // Toggle fullscreen off
                Thread.Sleep(1000);

                // Then pause
                if (IsVideoPlaying())
                {
                    player.SendKeys("k");
                    Thread.Sleep(500);
                    Console.WriteLine("[INFO] Video paused");
                }
                else
                {
                    Console.WriteLine("[INFO] Video already paused/ended");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARN] PauseVideo: {ex.Message}");
            }
        }

        public void MinimizeBrowser()
        {
            // Exit fullscreen if still in it
            try
            {
                var js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript("if(document.fullscreenElement) document.exitFullscreen();");
                Thread.Sleep(500);
            }
            catch { }

            _driver.Manage().Window.Minimize();
            Console.WriteLine("[INFO] Browser minimized");
            Thread.Sleep(1000);
        }

        public void CloseBrowser()
        {
            DriverManager.QuitDriver();
            Console.WriteLine("[INFO] Browser closed");
        }

        private bool IsElementVisible(By locator)
        {
            try { return _driver.FindElement(locator).Displayed; }
            catch { return false; }
        }

        private int ParseTimeToSeconds(string timeStr)
        {
            var parts = timeStr.Split(':');
            try
            {
                return parts.Length switch
                {
                    2 => int.Parse(parts[0]) * 60 + int.Parse(parts[1]),
                    3 => int.Parse(parts[0]) * 3600 + int.Parse(parts[1]) * 60 + int.Parse(parts[2]),
                    _ => 240
                };
            }
            catch { return 240; }
        }
    }
}
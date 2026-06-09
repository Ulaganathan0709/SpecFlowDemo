# 🎯 SpecFlow YouTube Automation Framework

[![Build Status](https://github.com/Ulaganathan0709/SpecFlowDemo/actions/workflows/test.yml/badge.svg)](https://github.com/Ulaganathan0709/SpecFlowDemo/actions/workflows/test.yml)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com)
[![SpecFlow](https://img.shields.io/badge/SpecFlow-3.9.74-00A9E0)](https://specflow.org)
[![Selenium](https://img.shields.io/badge/Selenium-4.18.1-43B02A?logo=selenium)](https://selenium.dev)
[![NUnit](https://img.shields.io/badge/NUnit-3.14.0-brightgreen)](https://nunit.org)
[![MailKit](https://img.shields.io/badge/MailKit-4.x-blue)](https://github.com/jstedfast/MailKit)
[![License](https://img.shields.io/badge/License-MIT-yellow)](LICENSE)

> A production-grade **BDD Test Automation Framework** built with SpecFlow + Selenium WebDriver + C# (.NET 8).  
> Automates YouTube music playback — search, fullscreen play, ad skip, duration tracking, pause, minimize, close.  
> Includes custom HTML report, FFmpeg screen recording, GIF generation, MailKit email notification, and CI/CD via GitHub Actions.

---

## 🎬 Test Results

```
✅ 3 Passed | 0 Failed | 100% Pass Rate | ~15 min Duration
```

| Song | Duration | Status |
|------|----------|--------|
| Maari Thara Local 4K | 3:20 | ✅ PASSED |
| Don'u Don'u Don'u | 3:07 | ✅ PASSED |
| Thappa Dhaan Theriyum | 3:22 | ✅ PASSED |

---

## 📋 Table of Contents

- [Tech Stack](#-tech-stack)
- [Project Structure](#-project-structure)
- [Features](#-features)
- [Prerequisites](#-prerequisites)
- [Installation](#-installation)
- [Configuration](#-configuration)
- [Running Tests](#-running-tests)
- [Test Report](#-test-report)
- [Screen Recording](#-screen-recording)
- [Email Report](#-email-report)
- [CI/CD Pipeline](#-cicd-pipeline)
- [Troubleshooting](#-troubleshooting)
- [CV Points](#-cv--resume-points)

---

## 🛠 Tech Stack

| Tool | Version | Purpose |
|------|---------|---------|
| C# / .NET | 8.0 | Language / Runtime |
| SpecFlow | 3.9.74 | BDD Framework (Gherkin) |
| Selenium WebDriver | 4.18.1 | Browser Automation |
| NUnit | 3.14.0 | Test Runner |
| ChromeDriver | 148.x | Chrome Browser Driver |
| FFmpeg | 8.1.1 | Screen recording + GIF |
| MailKit | 4.x | Email (SMTP port 465) |
| GitHub Actions | — | CI/CD Pipeline |

---

## 📁 Project Structure

```
SpecFlowDemo/
│
├── Features/
│   └── YoutubeMusic.feature          # Gherkin BDD scenarios (Scenario Outline)
│
├── Pages/
│   └── YoutubePage.cs                # Page Object Model
│
├── Steps/
│   └── YoutubeMusicSteps.cs          # Step definitions
│
├── Hooks/
│   └── Hooks.cs                      # Before/After hooks + email trigger
│
├── Utilities/
│   ├── DriverManager.cs              # ChromeDriver setup
│   ├── ReportManager.cs              # Custom HTML report generator
│   ├── ScreenRecorder.cs             # FFmpeg screen recorder + GIF
│   └── EmailReporter.cs              # MailKit email notification
│
├── Reports/
│   ├── TestReport.html               # Auto-generated dark-theme HTML report
│   └── Videos/
│       ├── scenario_name.mp4         # Full screen recording
│       └── scenario_name.gif         # 30sec GIF
│
├── .github/
│   └── workflows/
│       └── test.yml                  # GitHub Actions CI/CD
│
├── appsettings.json                  # Environment configuration
├── .gitignore
└── SpecFlowDemo.csproj               # NuGet dependencies
```

---

## ✨ Features

| Feature | Description |
|---------|-------------|
| ✅ BDD Gherkin | Human-readable feature files with Given/When/Then |
| ✅ Page Object Model | Clean separation — locators + actions in one class |
| ✅ Data-Driven Testing | Scenario Outline + Examples table — 3 songs in one run |
| ✅ Auto Ad Skip | Detects and skips YouTube ads automatically |
| ✅ Fullscreen Playback | F-key fullscreen automation during song playback |
| ✅ Duration Tracking | JavaScript executor — minute-by-minute progress logs |
| ✅ Smart Waits | Explicit WebDriverWait + Implicit waits |
| ✅ Screen Recording | FFmpeg auto-records every test scenario |
| ✅ GIF Generation | Auto-converts MP4 → GIF (30 sec preview) |
| ✅ Screenshot on Fail | Auto-captures screenshot on test failure |
| ✅ HTML Report | Dark-themed report with step-level logging + timestamps |
| ✅ Email Report | MailKit sends HTML report via SMTP port 465 |
| ✅ CI/CD Pipeline | GitHub Actions — runs on every push to main |

---

## 🔧 Prerequisites

### 1. .NET 8.0 SDK

```
https://dotnet.microsoft.com/download/dotnet/8.0
```

Verify:
```powershell
dotnet --version
# 8.0.x
```

---

### 2. Google Chrome (Latest)

```
https://www.google.com/chrome/
```

Check version:
```powershell
(Get-Item "C:\Program Files\Google\Chrome\Application\chrome.exe").VersionInfo.FileVersion
```

> **ChromeDriver Note:** If version mismatch error occurs:
> ```powershell
> # Example for Chrome 148:
> Invoke-WebRequest -Uri "https://storage.googleapis.com/chrome-for-testing-public/148.0.7778.217/win64/chromedriver-win64.zip" -OutFile "chromedriver.zip"
> Expand-Archive -Path "chromedriver.zip" -DestinationPath "." -Force
> Copy-Item ".\chromedriver-win64\chromedriver.exe" ".\bin\Debug\net8.0\chromedriver.exe" -Force
> ```

---

### 3. Git

```
https://git-scm.com/download/win
```

---

### 4. FFmpeg (Screen Recording)

```powershell
# Install Scoop
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
Invoke-RestMethod -Uri https://get.scoop.sh | Invoke-Expression

# Install FFmpeg
scoop install ffmpeg

# Verify
ffmpeg -version
```

---

### 5. Visual Studio Code (Optional)

```
https://code.visualstudio.com/
```

Recommended extensions: C# Dev Kit, SpecFlow for VS Code, GitLens

---

## 📦 Installation

### Step 1 — Clone

```powershell
git clone https://github.com/Ulaganathan0709/SpecFlowDemo.git
cd SpecFlowDemo
```

### Step 2 — Restore packages

```powershell
dotnet restore
```

### Step 3 — Build

```powershell
dotnet build
# Expected: Build succeeded. 0 Warning(s) 0 Error(s)
```

---

## ⚙️ Configuration

### Songs — `Features/YoutubeMusic.feature`

```gherkin
Examples:
  | songName                         |
  | Maari thara local 4k             |  ← Change songs here
  | don'u don'u don'u                |
  | Thappa dhaan theriyum video song |
```

### Environment — `appsettings.json`

```json
{
  "AppSettings": {
    "BaseUrl": "https://www.youtube.com",
    "Browser": "Chrome",
    "ImplicitWait": 10,
    "PageLoadTimeout": 60
  }
}
```

### Email — `Utilities/EmailReporter.cs`

```csharp
private const string FromEmail   = "your.email@gmail.com";
private const string AppPassword = "xxxx xxxx xxxx xxxx"; // Gmail App Password
private static readonly string[] ToEmails = {
    "recipient1@gmail.com",
    "recipient2@gmail.com"
};
```

**Gmail App Password steps:**
1. `myaccount.google.com` → Security → 2-Step Verification → ON
2. Search "App Passwords" → Generate
3. Copy 16-digit password → paste above

> **Note:** Uses SMTP port **465** (SSL) — port 587 may be blocked on some networks.

---

## 🚀 Running Tests

### Full sequence (recommended)

```powershell
dotnet restore
dotnet build
dotnet test
start "Reports\TestReport.html"
```

### Run all tests

```powershell
dotnet test
```

### Run with verbose logs

```powershell
dotnet test --logger "console;verbosity=detailed"
```

### Expected console output

```
[RECORD] ▶ Started → Reports/Videos/Play_songs_20260609_133519.mp4
[HOOK] Starting: Play songs and close browser after completion

[PASS] Browser launched — ChromeDriver started
[PASS] Navigated to https://www.youtube.com
[PASS] Searched for: Maari thara local 4k
[PASS] Opened video: Maari Thara Local 8K/4K Video Song | Dhanush | Anirudh
[PASS] Fullscreen enabled (f key)
[PASS] Ad detected and skipped
[PASS] Video playing confirmed: True
[PASS] Total duration: 00:03:20
[INFO] Progress → Played: 00:01:00 | Remaining: 00:02:20
[INFO] Progress → Played: 00:02:00 | Remaining: 00:01:20
[INFO] Progress → Played: 00:03:00 | Remaining: 00:00:20
[PASS] Song completed successfully ✓
[PASS] Video paused + Fullscreen exited
[PASS] Browser minimized

[RECORD] ✅ Saved → Reports/Videos/scenario.mp4 (67877KB)
[RECORD] 🎞 GIF  → Reports/Videos/scenario.gif (22299KB)

[HOOK] Scenario 3/3 done
[EMAIL] Connected via port 465 ✅
[EMAIL] Authenticated! ✅
[EMAIL] ✅ Sent to: recipient@gmail.com

Passed! - Failed: 0, Passed: 3, Skipped: 0, Total: 3, Duration: 15m 39s
```

---

## 📊 Test Report

Auto-generated at `Reports/TestReport.html` after every run.

```powershell
start "Reports\TestReport.html"
```

| Section | Details |
|---------|---------|
| Summary Cards | Pass, Fail, Total, Pass Rate %, Duration |
| Progress Bar | Green gradient bar |
| Scenario List | Expandable/collapsible per scenario |
| Step Timeline | Every step with ✅/❌/→ + timestamp |
| Color Coding | 🟢 Pass, 🔴 Fail, 🔵 Info |
| Screenshots | Auto-attached on failure |

---

## 🎥 Screen Recording

Every scenario auto-recorded via FFmpeg.

```
Reports/Videos/
├── Play_songs_20260609_133519.mp4    # ~67MB full recording
└── Play_songs_20260609_133519.gif    # ~22MB 30sec GIF
```

Adjust in `ScreenRecorder.cs`:
```
-framerate 10   # Lower = smaller file
-crf 35         # Higher = smaller, lower quality
-t 30           # GIF duration seconds
```

---

## 📧 Email Report

After all scenarios complete — HTML report auto-emailed via MailKit (port 465).

Email includes:
- Pass/Fail/Total/Pass Rate cards
- Per-scenario result table with badges
- `TestReport.html` attached
- GitHub repo link in footer

---

## 🔄 CI/CD Pipeline

`.github/workflows/test.yml` — runs on every push to `main`:

```yaml
name: YouTube Automation CI

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  test:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0'
      - name: Restore
        run: dotnet restore
      - name: Build
        run: dotnet build --no-restore
      - name: Test
        run: dotnet test --no-build --logger "trx;LogFileName=results.trx"
      - name: Upload Report
        uses: actions/upload-artifact@v4
        if: always()
        with:
          name: TestReport-${{ github.run_number }}
          path: Reports/TestReport.html
```

View runs: `github.com/Ulaganathan0709/SpecFlowDemo/actions`

---

## 🔀 Git Workflow

```powershell
# Daily push
git add .
git commit -m "Update test scenarios"
git push origin main
```

---

## 🐛 Troubleshooting

| Error | Fix |
|-------|-----|
| `ChromeDriver version mismatch` | Download matching chromedriver.exe — see Installation |
| `FFmpeg not found` | `scoop install ffmpeg` → restart PowerShell |
| `Failure sending mail` | Port 587 blocked — code uses port 465 (SSL) automatically |
| `Build failed — Console does not exist` | Add `<ImplicitUsings>enable</ImplicitUsings>` to csproj |
| `Video file corrupt` | `BeginErrorReadLine()` in ScreenRecorder.cs fixes this |
| `NuGet AventStack not found` | Not used — custom ReportManager has zero dependencies |

---

## 💡 Key Concepts Demonstrated

| Concept | Implementation |
|---------|---------------|
| BDD / Gherkin | `Features/YoutubeMusic.feature` |
| Page Object Model | `Pages/YoutubePage.cs` |
| Data-Driven Testing | `Scenario Outline` + `Examples` table |
| Explicit Wait | `WebDriverWait` + `ExpectedConditions` |
| JavaScript Executor | Video duration, currentTime, fullscreen exit |
| Hooks | `BeforeScenario` / `AfterScenario` / `AfterTestRun` |
| Screen Recording | `Utilities/ScreenRecorder.cs` — FFmpeg |
| Custom HTML Report | `Utilities/ReportManager.cs` — zero dependency |
| Email Notification | `Utilities/EmailReporter.cs` — MailKit port 465 |
| CI/CD | `.github/workflows/test.yml` — GitHub Actions |

---

## 📝 CV / Resume Points

```
Test Automation Engineer

• Built production-grade BDD automation framework using SpecFlow 3.9.74 +
  Selenium WebDriver 4.18.1 + C# (.NET 8) with Page Object Model design pattern

• Implemented Data-Driven Testing via SpecFlow Scenario Outline —
  3 test cases executed from single scenario using Examples table

• Automated YouTube end-to-end flow: search → fullscreen play →
  ad skip → minute-by-minute duration tracking → pause → minimize → close

• Integrated FFmpeg screen recording — auto-records every test run,
  generates MP4 + GIF artifacts for documentation

• Developed custom dark-themed HTML report with step-level logging,
  timestamp tracking, progress bar, and failure screenshot attachment

• Implemented MailKit email notification (SMTP port 465) —
  sends HTML report with pass/fail summary to multiple recipients

• Set up CI/CD pipeline using GitHub Actions — auto-runs on push to main,
  uploads TestReport.html as downloadable artifact

GitHub: https://github.com/Ulaganathan0709/SpecFlowDemo
```

---

## 👤 Author

**Ulaganathan**  
Automation Engineer  
GitHub: [@Ulaganathan0709](https://github.com/Ulaganathan0709)  
Email: ulaganathana7@gmail.com

---

## 📄 License

MIT License — free to use, modify, and distribute.

---

*Built with ❤️ using SpecFlow + Selenium + C# — from scratch to production!*
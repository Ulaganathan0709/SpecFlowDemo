# 🎯 SpecFlow YouTube Automation Framework

![Build Status](https://github.com/YOUR_USERNAME/SpecFlowDemo/actions/workflows/test.yml/badge.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![SpecFlow](https://img.shields.io/badge/SpecFlow-3.9.74-00A9E0)
![Selenium](https://img.shields.io/badge/Selenium-4.18.1-43B02A?logo=selenium)
![NUnit](https://img.shields.io/badge/NUnit-3.14.0-brightgreen)
![License](https://img.shields.io/badge/License-MIT-yellow)

> A production-grade **BDD Test Automation Framework** built with SpecFlow + Selenium WebDriver + C# (.NET 8).  
> Automates YouTube music playback — search, fullscreen play, ad skip, duration tracking, pause, minimize, close.  
> Includes custom HTML report, screen recording, GIF generation, email notification, and CI/CD via GitHub Actions.

---

## 🎬 Demo

> Screen recorded automatically during test execution

```
✅ 1 Passed | 0 Failed | 100% Pass Rate | 6:32 Duration
```

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
- [CV Points](#-cv--resume-points)

---

## 🛠 Tech Stack

| Tool | Version | Purpose |
|------|---------|---------|
| C# / .NET | 8.0 | Language / Runtime |
| SpecFlow | 3.9.74 | BDD Framework (Gherkin) |
| Selenium WebDriver | 4.18.1 | Browser Automation |
| NUnit | 3.14.0 | Test Runner |
| Selenium Manager | Auto | ChromeDriver auto-management |
| FFmpeg | Latest | Screen recording + GIF |
| GitHub Actions | — | CI/CD Pipeline |

---

## 📁 Project Structure

```
SpecFlowDemo/
│
├── Features/
│   └── YoutubeMusic.feature          # Gherkin BDD scenarios
│
├── Pages/
│   └── YoutubePage.cs                # Page Object Model
│
├── Steps/
│   └── YoutubeMusicSteps.cs          # Step definitions
│
├── Hooks/
│   └── Hooks.cs                      # Before/After hooks
│
├── Utilities/
│   ├── DriverManager.cs              # ChromeDriver setup
│   ├── ReportManager.cs              # HTML report generator
│   ├── ScreenRecorder.cs             # FFmpeg screen recorder
│   └── EmailReporter.cs              # Email notification
│
├── Reports/
│   ├── TestReport.html               # Auto-generated HTML report
│   └── Videos/
│       ├── scenario_name.mp4         # Full screen recording
│       └── scenario_name.gif         # GIF for README embed
│
├── .github/
│   └── workflows/
│       └── test.yml                  # GitHub Actions CI/CD
│
├── allureConfig.json                 # Allure config (optional)
├── appsettings.json                  # Environment config
└── SpecFlowDemo.csproj               # Project dependencies
```

---

## ✨ Features

| Feature | Description |
|---------|-------------|
| ✅ BDD Gherkin | Human-readable feature files with Given/When/Then |
| ✅ Page Object Model | Clean separation — locators + actions in one class |
| ✅ Data-Driven Testing | Scenario Outline + Examples table — multiple songs |
| ✅ Auto Ad Skip | Detects and skips YouTube ads automatically |
| ✅ Fullscreen Playback | F-key fullscreen during song playback |
| ✅ Duration Tracking | JavaScript executor — minute-by-minute progress |
| ✅ Smart Waits | Explicit WebDriverWait + Implicit waits |
| ✅ Screen Recording | FFmpeg auto-records every test run |
| ✅ GIF Generation | Auto-converts MP4 → GIF for README embed |
| ✅ Screenshot on Fail | Auto-captures screenshot on test failure |
| ✅ HTML Report | Dark-themed report with step-level logging |
| ✅ Email Report | Auto-sends HTML report after test run |
| ✅ CI/CD Pipeline | GitHub Actions — runs on every push |

---

## 🔧 Prerequisites

### 1. .NET 8.0 SDK

Download and install from:
```
https://dotnet.microsoft.com/download/dotnet/8.0
```

Verify installation:
```powershell
dotnet --version
# Should show: 8.0.x
```

---

### 2. Google Chrome (Latest)

Download from:
```
https://www.google.com/chrome/
```

Check version:
```powershell
(Get-Item "C:\Program Files\Google\Chrome\Application\chrome.exe").VersionInfo.FileVersion
# Example: 148.0.7778.217
```

> **Note:** ChromeDriver is auto-managed by Selenium Manager — no manual download needed!  
> But if version mismatch occurs, download matching ChromeDriver from:  
> `https://googlechromelabs.github.io/chrome-for-testing/`  
> Place `chromedriver.exe` in project root folder.

---

### 3. Git

Download from:
```
https://git-scm.com/download/win
```

Verify:
```powershell
git --version
# Should show: git version 2.x.x
```

---

### 4. FFmpeg (For Screen Recording)

Install via Scoop (recommended):
```powershell
# Step 1: Install Scoop
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
Invoke-RestMethod -Uri https://get.scoop.sh | Invoke-Expression

# Step 2: Install FFmpeg
scoop install ffmpeg

# Verify
ffmpeg -version
# Should show: ffmpeg version 8.x.x
```

---

### 5. Visual Studio Code (Optional but recommended)

Download from:
```
https://code.visualstudio.com/
```

Recommended extensions:
- C# Dev Kit
- SpecFlow for Visual Studio Code
- GitLens

---

## 📦 Installation

### Step 1 — Clone the repository

```powershell
git clone https://github.com/YOUR_USERNAME/SpecFlowDemo.git
cd SpecFlowDemo
```

---

### Step 2 — Restore NuGet packages

```powershell
dotnet restore
```

Expected output:
```
Restored C:\...\SpecFlowDemo.csproj (in 622 ms)
```

---

### Step 3 — Build the project

```powershell
dotnet build
```

Expected output:
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

---

### Step 4 — ChromeDriver setup (if needed)

If you see `ChromeDriver version mismatch` error:

```powershell
# Check your Chrome version
(Get-Item "C:\Program Files\Google\Chrome\Application\chrome.exe").VersionInfo.FileVersion

# Download matching ChromeDriver (replace VERSION with your chrome version)
# Example for Chrome 148:
Invoke-WebRequest -Uri "https://storage.googleapis.com/chrome-for-testing-public/148.0.7778.217/win64/chromedriver-win64.zip" -OutFile "chromedriver.zip"
Expand-Archive -Path "chromedriver.zip" -DestinationPath "." -Force
Copy-Item ".\chromedriver-win64\chromedriver.exe" ".\bin\Debug\net8.0\chromedriver.exe" -Force
Copy-Item ".\chromedriver-win64\chromedriver.exe" ".\chromedriver.exe" -Force
```

---

## ⚙️ Configuration

### `appsettings.json` — Environment settings

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

---

### `Features/YoutubeMusic.feature` — Add/change songs

```gherkin
Examples:
  | songName                          |
  | Voda Voda Dhooram Korayala        |   ← Change songs here
  | Excuse Me Mr Kandasamy 4K         |
  | Rowdy Baby 4K Video Song          |
```

---

### Email configuration — `Utilities/EmailReporter.cs`

```csharp
private const string FromEmail   = "your.email@gmail.com";
private const string AppPassword = "xxxx xxxx xxxx xxxx";  // Gmail App Password
private const string ToEmail     = "recipient@gmail.com";
```

**Gmail App Password generate panra steps:**
1. gmail.com → Google Account → Security
2. Enable 2-Step Verification
3. Search "App Passwords" → Generate
4. 16-digit password copy pannu → paste here

---

## 🚀 Running Tests

### Run all tests

```powershell
dotnet test
```

---

### Run with detailed logs

```powershell
dotnet test --logger "console;verbosity=detailed"
```

---

### Run specific tag only

```powershell
dotnet test --filter "Category=Youtube"
```

---

### Run and open report

```powershell
dotnet test
start "Reports\TestReport.html"
```

---

### Full sequence (recommended)

```powershell
dotnet restore
dotnet build
dotnet test
start "Reports\TestReport.html"
```

---

### Expected console output

```
[RECORD] ▶ Started → Reports/Videos/scenario_name.mp4
[HOOK] Starting: Play songs and close browser after completion

[PASS] Browser launched — ChromeDriver started
[PASS] Navigated to https://www.youtube.com
[PASS] Searched for: Voda Voda Dhooram Korayala
[PASS] Opened video: Voda Voda Dhooram Korayala...
[PASS] Fullscreen enabled (f key)
[PASS] Ad detected and skipped
[PASS] Video playing confirmed: True
[PASS] Total duration: 00:04:05
[INFO] Progress → Played: 00:01:00 | Remaining: 00:03:05
[INFO] Progress → Played: 00:02:00 | Remaining: 00:02:05
[INFO] Progress → Played: 00:03:00 | Remaining: 00:01:05
[PASS] Song completed successfully ✓
[PASS] Video paused + Fullscreen exited
[PASS] Browser minimized
[PASS] Browser closing...

[RECORD] ⏹ Saved → Reports/Videos/scenario.mp4 (172534KB)
[RECORD] 🎞 GIF → Reports/Videos/scenario.gif

Passed! - Failed: 0, Passed: 1, Skipped: 0, Total: 1, Duration: 6m 32s
```

---

## 📊 Test Report

Auto-generated at `Reports/TestReport.html` after every run.

### Open report

```powershell
start "Reports\TestReport.html"
```

### Report contains

| Section | Details |
|---------|---------|
| Summary Cards | Pass count, Fail count, Total, Pass Rate %, Duration |
| Progress Bar | Green bar showing pass percentage |
| Scenario List | Each scenario expandable/collapsible |
| Step Timeline | Every step with status + timestamp |
| Color Coding | 🟢 Pass, 🔴 Fail, 🔵 Info |
| Screenshots | Auto-attached on failure |

---

## 🎥 Screen Recording

Every test run is automatically recorded.

### Output files

```
Reports/
└── Videos/
    ├── Play_songs_20260609_100419.mp4    # Full recording (~172MB)
    └── Play_songs_20260609_100419.gif    # 30sec GIF (~5MB)
```

### Manual play

```powershell
# Open MP4
start "Reports\Videos\Play_songs_20260609_100419.mp4"
```

### Recording settings (adjust in `ScreenRecorder.cs`)

```csharp
-framerate 10    // Lower = smaller file (default: 10fps)
-crf 35          // Higher = smaller file, lower quality (default: 35)
-t 30            // GIF duration in seconds (default: 30sec)
```

---

## 📧 Email Report

After every test run, HTML report auto-emailed.

### Email contains

- ✅ Pass/Fail summary cards with colors
- ✅ Per-scenario result table with badges
- ✅ TestReport.html attached
- ✅ Professional HTML email template

### Setup steps

1. Gmail → Google Account → Security → App Passwords → Generate
2. Copy 16-digit password
3. Open `Utilities/EmailReporter.cs`
4. Fill `FromEmail`, `AppPassword`, `ToEmail`
5. Run `dotnet test` — email auto-sends!

---

## 🔄 CI/CD Pipeline

### GitHub Actions — `.github/workflows/test.yml`

```yaml
name: YouTube Automation

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]
  schedule:
    - cron: '0 3 * * *'   # Daily 8:30AM IST

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
        run: dotnet build
      - name: Test
        run: dotnet test --logger "trx;LogFileName=results.trx"
      - name: Upload Report
        uses: actions/upload-artifact@v4
        with:
          name: TestReport
          path: Reports/TestReport.html
```

### Setup GitHub Actions

```powershell
# 1. Create .github/workflows folder
mkdir .github\workflows

# 2. Create test.yml (paste above content)

# 3. Push to GitHub
git add .
git commit -m "Add CI/CD pipeline"
git push origin main
```

### View results

```
github.com/YOUR_USERNAME/SpecFlowDemo
→ Actions tab
→ See all runs with green ✅ or red ❌
→ Click any run → Download TestReport artifact
```

---

## 🔀 Git Commands

### First time setup

```powershell
# Initialize git
git init

# Create .gitignore
@"
bin/
obj/
Reports/Videos/
*.user
.vs/
chromedriver.zip
chromedriver-win64/
"@ | Out-File .gitignore -Encoding UTF8

# Add all files
git add .
git commit -m "Initial commit — SpecFlow YouTube automation framework"

# Connect to GitHub (create repo on github.com first)
git remote add origin https://github.com/YOUR_USERNAME/SpecFlowDemo.git
git branch -M main
git push -u origin main
```

### Daily workflow

```powershell
git add .
git commit -m "Your message here"
git push
```

---

## 🐛 Troubleshooting

### ChromeDriver version mismatch
```
Error: This version of ChromeDriver only supports Chrome version 123
Fix: Download matching chromedriver.exe — see Installation Step 4
```

### FFmpeg not found
```
Error: [RECORD] ⚠ Start error: The system cannot find the file specified
Fix: scoop install ffmpeg
     Restart PowerShell after install
```

### NuGet restore failed
```
Error: Unable to find package AventStack.ExtentReports
Fix: This package is NOT used — remove from csproj if present
     Our framework uses custom ReportManager (zero dependency)
```

### Build failed — 'Console' does not exist
```
Error: CS0103: The name 'Console' does not exist
Fix: Add to csproj PropertyGroup:
     <ImplicitUsings>enable</ImplicitUsings>
     Or add 'using System;' at top of each file
```

### Video file corrupt / won't play
```
Error: 0xC00D36C4 — file unsupported
Fix: FFmpeg process didn't stop cleanly
     Updated ScreenRecorder.cs fixes this — BeginErrorReadLine() added
```

---

## 💡 Key Concepts Demonstrated

| Concept | Where |
|---------|-------|
| BDD / Gherkin | `Features/YoutubeMusic.feature` |
| Page Object Model | `Pages/YoutubePage.cs` |
| Data-Driven Testing | `Scenario Outline` + `Examples` table |
| Explicit Wait | `WebDriverWait` in Steps |
| JavaScript Executor | Video duration, currentTime, fullscreen |
| Hooks | `Hooks.cs` — BeforeScenario, AfterScenario |
| Screen Recording | `Utilities/ScreenRecorder.cs` |
| Custom Reporting | `Utilities/ReportManager.cs` |
| Email Notification | `Utilities/EmailReporter.cs` |
| CI/CD | `.github/workflows/test.yml` |

---

## 📝 CV / Resume Points

```
Test Automation Engineer

• Built end-to-end BDD automation framework using SpecFlow 3.9.74 +
  Selenium WebDriver 4.x + C# (.NET 8) with Page Object Model design pattern

• Implemented Data-Driven Testing via SpecFlow Scenario Outline —
  executed multiple test cases from single scenario using Examples table

• Automated YouTube end-to-end flow: search → fullscreen play →
  ad skip → duration tracking → pause → minimize → close

• Integrated FFmpeg screen recording — auto-records every test run,
  generates MP4 + GIF for documentation and CI/CD artifacts

• Developed custom dark-themed HTML test report with step-level
  logging, timestamp tracking, and failure screenshot attachment

• Implemented automated email notification — sends HTML report
  with pass/fail summary after every test execution

• Set up CI/CD pipeline using GitHub Actions — auto-runs on
  push to main branch with report artifact upload

GitHub: https://github.com/YOUR_USERNAME/SpecFlowDemo
```

---

## 👤 Author

**Ulaga**  
Automation Engineer  
GitHub: [@YOUR_USERNAME](https://github.com/YOUR_USERNAME)  
Email: your.email@gmail.com

---

## 📄 License

MIT License — free to use, modify, and distribute.

---

*Built with ❤️ using SpecFlow + Selenium + C#*
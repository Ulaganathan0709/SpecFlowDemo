using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpecFlowDemo.Utilities
{
    public static class ReportManager
    {
        private static readonly string ReportDir = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Reports");

        private static readonly string ReportPath =
            Path.Combine(ReportDir, "TestReport.html");

        public record StepLog(string Status, string Text, string Time);
        public record ScenarioResult(
            string Name, string Status, string Duration,
            string StartTime, string EndTime,
            List<StepLog> Steps, string? ScreenshotPath, string? ErrorMessage);

        private static readonly List<ScenarioResult> _results = new();
        private static DateTime _suiteStart;

        public static void Initialize()
        {
            Directory.CreateDirectory(ReportDir);
            _results.Clear();
            _suiteStart = DateTime.Now;
        }

        public static void LogPass(string name, List<StepLog> steps,
            DateTime start, DateTime end)
        {
            _results.Add(new ScenarioResult(
                name, "PASSED",
                (end - start).ToString(@"mm\:ss\.ff"),
                start.ToString("HH:mm:ss"),
                end.ToString("HH:mm:ss"),
                steps, null, null));
        }

        public static void LogFail(string name, string error,
            List<StepLog> steps, DateTime start, DateTime end,
            string? screenshotPath = null)
        {
            _results.Add(new ScenarioResult(
                name, "FAILED",
                (end - start).ToString(@"mm\:ss\.ff"),
                start.ToString("HH:mm:ss"),
                end.ToString("HH:mm:ss"),
                steps, screenshotPath, error));
        }

        public static List<ScenarioResult> GetResults()
        {
            return _results;
        }

        public static void Flush()
        {
            int pass  = _results.Count(r => r.Status == "PASSED");
            int fail  = _results.Count(r => r.Status == "FAILED");
            int total = _results.Count;
            int pct   = total > 0 ? (int)Math.Round(pass * 100.0 / total) : 0;
            string dur = (DateTime.Now - _suiteStart).ToString(@"mm\:ss");

            var scenarioCards = new StringBuilder();
            foreach (var r in _results)
            {
                string statusClass = r.Status == "PASSED" ? "pass" : "fail";
                string statusIcon  = r.Status == "PASSED" ? "✓" : "✗";

                var stepRows = new StringBuilder();
                foreach (var s in r.Steps)
                {
                    string sc = s.Status == "pass" ? "step-pass"
                              : s.Status == "fail" ? "step-fail"
                              : "step-info";
                    string si = s.Status == "pass" ? "✓"
                              : s.Status == "fail" ? "✗" : "→";
                    stepRows.Append($@"
                    <div class='step-row {sc}'>
                      <span class='step-icon'>{si}</span>
                      <span class='step-text'>{s.Text}</span>
                      <span class='step-time'>{s.Time}</span>
                    </div>");
                }

                string screenshot = r.ScreenshotPath != null
                    ? $"<div class='screenshot'><img src='{r.ScreenshotPath}' alt='Failure Screenshot'/></div>"
                    : "";

                string errorBlock = r.ErrorMessage != null
                    ? $"<div class='error-block'><i>⚠</i> {r.ErrorMessage}</div>"
                    : "";

                scenarioCards.Append($@"
                <div class='scenario-card {statusClass}-card'>
                  <div class='scenario-header' onclick='toggle(this)'>
                    <div class='scenario-left'>
                      <span class='status-badge {statusClass}-badge'>{statusIcon} {r.Status}</span>
                      <span class='scenario-name'>{r.Name}</span>
                    </div>
                    <div class='scenario-right'>
                      <span class='meta'>⏱ {r.Duration}</span>
                      <span class='meta'>▶ {r.StartTime}</span>
                      <span class='meta'>■ {r.EndTime}</span>
                      <span class='chevron'>▾</span>
                    </div>
                  </div>
                  <div class='scenario-body'>
                    <div class='steps-container'>
                      {stepRows}
                    </div>
                    {errorBlock}
                    {screenshot}
                  </div>
                </div>");
            }

            string html = $@"<!DOCTYPE html>
<html lang='en'>
<head>
<meta charset='UTF-8'/>
<meta name='viewport' content='width=device-width, initial-scale=1'/>
<title>Test Report — {DateTime.Now:dd MMM yyyy}</title>
<style>
  *{{box-sizing:border-box;margin:0;padding:0}}
  body{{font-family:'Segoe UI',sans-serif;background:#0d1117;color:#c9d1d9;min-height:100vh}}
  .topbar{{background:#161b22;border-bottom:1px solid #30363d;padding:16px 32px;display:flex;align-items:center;justify-content:space-between}}
  .topbar h1{{font-size:18px;font-weight:500;color:#e6edf3}}
  .topbar .meta{{font-size:12px;color:#8b949e}}
  .container{{max-width:1100px;margin:0 auto;padding:28px 24px}}
  .summary{{display:grid;grid-template-columns:repeat(5,1fr);gap:14px;margin-bottom:32px}}
  .card{{background:#161b22;border:1px solid #30363d;border-radius:12px;padding:20px;text-align:center}}
  .card .num{{font-size:32px;font-weight:600;margin-bottom:4px}}
  .card .lbl{{font-size:11px;color:#8b949e;text-transform:uppercase;letter-spacing:.08em}}
  .pass-num{{color:#3fb950}}.fail-num{{color:#f85149}}
  .total-num{{color:#58a6ff}}.pct-num{{color:#e3b341}}.dur-num{{color:#bc8cff}}
  .progress-bar{{background:#21262d;border-radius:99px;height:8px;margin-bottom:32px;overflow:hidden}}
  .progress-fill{{height:100%;border-radius:99px;background:linear-gradient(90deg,#3fb950,#2ea043);transition:width .6s ease}}
  .section-title{{font-size:13px;font-weight:500;color:#8b949e;text-transform:uppercase;
    letter-spacing:.08em;margin-bottom:14px}}
  .scenario-card{{border:1px solid #30363d;border-radius:10px;margin-bottom:12px;overflow:hidden}}
  .pass-card{{border-left:3px solid #3fb950}}
  .fail-card{{border-left:3px solid #f85149}}
  .scenario-header{{display:flex;align-items:center;justify-content:space-between;
    padding:14px 18px;cursor:pointer;background:#161b22;user-select:none}}
  .scenario-header:hover{{background:#1c2128}}
  .scenario-left{{display:flex;align-items:center;gap:12px}}
  .scenario-right{{display:flex;align-items:center;gap:16px}}
  .scenario-name{{font-size:14px;font-weight:500;color:#e6edf3}}
  .status-badge{{font-size:11px;font-weight:600;padding:3px 10px;border-radius:20px;letter-spacing:.04em}}
  .pass-badge{{background:#0d3321;color:#3fb950;border:1px solid #238636}}
  .fail-badge{{background:#3d0c0c;color:#f85149;border:1px solid #6e1717}}
  .meta{{font-size:11px;color:#8b949e}}
  .chevron{{color:#8b949e;font-size:12px;transition:transform .2s}}
  .chevron.open{{transform:rotate(180deg)}}
  .scenario-body{{display:none;padding:16px 18px;background:#0d1117;
    border-top:1px solid #21262d}}
  .scenario-body.open{{display:block}}
  .steps-container{{display:flex;flex-direction:column;gap:4px;margin-bottom:12px}}
  .step-row{{display:flex;align-items:flex-start;gap:10px;padding:8px 12px;
    border-radius:6px;font-size:13px}}
  .step-pass{{background:#0d1f0d;border-left:2px solid #3fb950}}
  .step-fail{{background:#1f0d0d;border-left:2px solid #f85149}}
  .step-info{{background:#0d1520;border-left:2px solid #58a6ff}}
  .step-icon{{font-size:12px;margin-top:1px;flex-shrink:0}}
  .step-pass .step-icon{{color:#3fb950}}
  .step-fail .step-icon{{color:#f85149}}
  .step-info .step-icon{{color:#58a6ff}}
  .step-text{{flex:1;color:#c9d1d9;line-height:1.5}}
  .step-time{{font-size:11px;color:#8b949e;flex-shrink:0}}
  .error-block{{background:#2d1515;border:1px solid #6e1717;border-radius:6px;
    padding:12px;font-size:12px;color:#f85149;margin-top:8px;word-break:break-all}}
  .screenshot img{{max-width:100%;border-radius:6px;margin-top:12px;
    border:1px solid #30363d}}
  .footer{{text-align:center;font-size:12px;color:#8b949e;padding:24px;
    border-top:1px solid #21262d;margin-top:32px}}
</style>
</head>
<body>
<div class='topbar'>
  <h1>🎯 SpecFlow Automation Report</h1>
  <span class='meta'>Generated: {DateTime.Now:dd MMM yyyy, HH:mm:ss}</span>
</div>
<div class='container'>
  <div class='summary'>
    <div class='card'><div class='num pass-num'>{pass}</div><div class='lbl'>Passed</div></div>
    <div class='card'><div class='num fail-num'>{fail}</div><div class='lbl'>Failed</div></div>
    <div class='card'><div class='num total-num'>{total}</div><div class='lbl'>Total</div></div>
    <div class='card'><div class='num pct-num'>{pct}%</div><div class='lbl'>Pass Rate</div></div>
    <div class='card'><div class='num dur-num'>{dur}</div><div class='lbl'>Duration</div></div>
  </div>
  <div class='progress-bar'>
    <div class='progress-fill' style='width:{pct}%'></div>
  </div>
  <div class='section-title'>Scenarios ({total})</div>
  {scenarioCards}
  <div class='footer'>SpecFlow + Selenium + NUnit | YouTube Automation Suite</div>
</div>
<script>
function toggle(header){{
  const body = header.nextElementSibling;
  const chevron = header.querySelector('.chevron');
  body.classList.toggle('open');
  chevron.classList.toggle('open');
}}
// Auto-open failed scenarios
document.querySelectorAll('.fail-card .scenario-header').forEach(h => toggle(h));
</script>
</body>
</html>";

            File.WriteAllText(ReportPath, html);
            Console.WriteLine($"[REPORT] → {ReportPath}");
        }
    }
}
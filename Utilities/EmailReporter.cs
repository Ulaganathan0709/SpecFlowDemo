using System;
using System.IO;
using System.Threading;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace SpecFlowDemo.Utilities
{
    public static class EmailReporter
    {
        private const string FromEmail   = "ulaganathana7@gmail.com";
        private const string AppPassword = "tlvukzneqmuvmoqm";
        private static readonly string[] ToEmails = {
            "ulaganathan996@gmail.com",
            "rsivakami98@gmail.com",
            "sathiyarajgm21@gmail.com",
        };

        public static void SendReport(string reportPath, int pass, int fail)
        {
            try
            {
                Console.WriteLine("[EMAIL] Preparing email...");

                int    total  = pass + fail;
                int    pct    = total > 0
                    ? (int)Math.Round(pass * 100.0 / total) : 0;
                string status = fail == 0 ? "PASSED" : "FAILED";
                string color  = fail == 0 ? "#1a7f37" : "#cf222e";

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(
                    "SpecFlow Automation", FromEmail));
                foreach (var email in ToEmails)
                {
                    message.To.Add(MailboxAddress.Parse(email));
                    }
                message.Subject =
                    $"[Automation] YouTube Tests — {status} | " +
                    $"{pass}/{total} | {DateTime.Now:dd MMM yyyy HH:mm}";

                var rows = new System.Text.StringBuilder();
                foreach (var r in ReportManager.GetResults())
                {
                    string badge = r.Status == "PASSED"
                        ? "<span style='background:#dafbe1;color:#1a7f37;" +
                          "padding:2px 8px;border-radius:20px;" +
                          "font-size:11px;font-weight:600'>PASSED</span>"
                        : "<span style='background:#ffebe9;color:#cf222e;" +
                          "padding:2px 8px;border-radius:20px;" +
                          "font-size:11px;font-weight:600'>FAILED</span>";

                    rows.Append($@"
                    <tr>
                      <td style='padding:10px 12px;border:1px solid #d0d7de;
                                 font-size:13px'>{r.Name}</td>
                      <td style='padding:10px 12px;border:1px solid #d0d7de;
                                 text-align:center'>{badge}</td>
                      <td style='padding:10px 12px;border:1px solid #d0d7de;
                                 font-size:13px;text-align:center'>
                                 {r.Duration}</td>
                    </tr>");
                }

                string htmlBody = $@"
<!DOCTYPE html>
<html>
<head><meta charset='UTF-8'/></head>
<body style='font-family:Segoe UI,sans-serif;
             background:#f4f6f9;padding:24px;margin:0'>
<div style='max-width:620px;margin:0 auto;background:#fff;
            border-radius:12px;overflow:hidden;
            box-shadow:0 2px 12px rgba(0,0,0,.08)'>
  <div style='background:{color};color:#fff;padding:28px 32px'>
    <h1 style='margin:0;font-size:22px;font-weight:500'>
      YouTube Automation Report</h1>
    <p style='margin:8px 0 0;font-size:13px;opacity:.85'>
      {DateTime.Now:dddd, dd MMMM yyyy HH:mm:ss}</p>
  </div>
  <div style='padding:24px 32px 0'>
    <table style='width:100%;border-collapse:separate;border-spacing:10px'>
      <tr>
        <td style='background:#f0fff4;border:1px solid #bbf7d0;
                   border-radius:10px;padding:16px;text-align:center'>
          <div style='font-size:32px;font-weight:700;color:#15803d'>
            {pass}</div>
          <div style='font-size:11px;color:#57606a;
                      text-transform:uppercase'>Passed</div>
        </td>
        <td style='background:#fff5f5;border:1px solid #fecaca;
                   border-radius:10px;padding:16px;text-align:center'>
          <div style='font-size:32px;font-weight:700;color:#dc2626'>
            {fail}</div>
          <div style='font-size:11px;color:#57606a;
                      text-transform:uppercase'>Failed</div>
        </td>
        <td style='background:#eff6ff;border:1px solid #bfdbfe;
                   border-radius:10px;padding:16px;text-align:center'>
          <div style='font-size:32px;font-weight:700;color:#1d4ed8'>
            {total}</div>
          <div style='font-size:11px;color:#57606a;
                      text-transform:uppercase'>Total</div>
        </td>
        <td style='background:#fffbeb;border:1px solid #fde68a;
                   border-radius:10px;padding:16px;text-align:center'>
          <div style='font-size:32px;font-weight:700;color:#d97706'>
            {pct}%</div>
          <div style='font-size:11px;color:#57606a;
                      text-transform:uppercase'>Pass Rate</div>
        </td>
      </tr>
    </table>
  </div>
  <div style='padding:20px 32px'>
    <div style='background:#e5e7eb;border-radius:99px;
                height:12px;overflow:hidden'>
      <div style='height:100%;width:{pct}%;border-radius:99px;
                  background:linear-gradient(90deg,#22c55e,#15803d)'>
      </div>
    </div>
    <p style='font-size:12px;color:#6b7280;margin:6px 0 0;text-align:right'>
      {pass} of {total} tests passed</p>
  </div>
  <div style='padding:0 32px 24px'>
    <table style='width:100%;border-collapse:collapse'>
      <thead>
        <tr>
          <th style='background:#f9fafb;color:#6b7280;text-align:left;
                     padding:10px 12px;border:1px solid #e5e7eb;
                     font-size:11px;text-transform:uppercase'>Scenario</th>
          <th style='background:#f9fafb;color:#6b7280;text-align:center;
                     padding:10px 12px;border:1px solid #e5e7eb;
                     font-size:11px;text-transform:uppercase;
                     width:100px'>Status</th>
          <th style='background:#f9fafb;color:#6b7280;text-align:center;
                     padding:10px 12px;border:1px solid #e5e7eb;
                     font-size:11px;text-transform:uppercase;
                     width:100px'>Duration</th>
        </tr>
      </thead>
      <tbody>{rows}</tbody>
    </table>
  </div>
  <div style='background:#f9fafb;border-top:1px solid #e5e7eb;
              padding:16px 32px;text-align:center'>
    <p style='font-size:12px;color:#9ca3af;margin:0'>
      SpecFlow 3.9.74 + Selenium WebDriver 4.18.1 + NUnit 3.14.0</p>
    <p style='font-size:12px;color:#9ca3af;margin:4px 0 0'>
      <a href='https://github.com/Ulaganathan0709/SpecFlowDemo'
         style='color:#6b7280'>GitHub Repository</a></p>
  </div>
</div>
</body>
</html>";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlBody
                };

                // Wait for file release
                Thread.Sleep(1000);

                if (File.Exists(reportPath))
                {
                    bodyBuilder.Attachments.Add(reportPath);
                    Console.WriteLine("[EMAIL] Report attached");
                }

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();

                Console.WriteLine("[EMAIL] Connecting port 465...");
                client.Connect(
                    "smtp.gmail.com", 465,
                    SecureSocketOptions.SslOnConnect);
                Console.WriteLine("[EMAIL] Connected!");

                client.Authenticate(FromEmail, AppPassword);
                Console.WriteLine("[EMAIL] Authenticated!");

                client.Send(message);
                client.Disconnect(true);

                Console.WriteLine($"[EMAIL] ✅ Sent to: {string.Join(", ", ToEmails)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EMAIL] ❌ Failed: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine(
                        $"[EMAIL] Inner: {ex.InnerException.Message}");
            }
        }
    }
}
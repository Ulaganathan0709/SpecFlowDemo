using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace SpecFlowDemo.Utilities
{
    public static class ScreenRecorder
    {
        private static Process?  _ffmpegProcess;
        private static string?   _outputPath;

        public static void StartRecording(string scenarioName)
        {
            try
            {
                string safeName = string.Join("_",
                    scenarioName.Split(
                        Path.GetInvalidFileNameChars()));

                string recordDir = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "..", "..", "..", "Reports", "Videos");

                Directory.CreateDirectory(recordDir);

                _outputPath = Path.Combine(recordDir,
                    $"{safeName}_{DateTime.Now:yyyyMMdd_HHmmss}.mp4");

                var startInfo = new ProcessStartInfo
                {
                    FileName               = "ffmpeg",
                    Arguments =
                    $"-y -f gdigrab -framerate 10 " +  // 20 → 10
                    $"-draw_mouse 1 -i desktop " +
                    $"-c:v libx264 -preset ultrafast " +
                    $"-crf 35 " +                       // 28 → 35 (smaller file)
                    $"-pix_fmt yuv420p " +
                    $"-movflags +faststart " +
                    $"\"{_outputPath}\"",
                    UseShellExecute        = false,
                    CreateNoWindow         = true,
                    RedirectStandardInput  = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError  = true
                };

                _ffmpegProcess = new Process
                {
                    StartInfo = startInfo
                };
                _ffmpegProcess.Start();

                // Drain stderr — without this FFmpeg blocks
                _ffmpegProcess.BeginErrorReadLine();

                Thread.Sleep(1000); // Wait for FFmpeg to init
                Console.WriteLine(
                    $"[RECORD] ▶ Started → {_outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[RECORD] ⚠ Start error: {ex.Message}");
            }
        }

        public static string? StopRecording()
        {
            if (_ffmpegProcess == null || _ffmpegProcess.HasExited)
                return _outputPath;

            try
            {
                Console.WriteLine("[RECORD] ⏹ Stopping...");

                // Send 'q' — graceful FFmpeg stop
                _ffmpegProcess.StandardInput.Write("q");
                _ffmpegProcess.StandardInput.Flush();

                // Wait max 10 seconds for clean exit
                bool exited = _ffmpegProcess.WaitForExit(10000);

                if (!exited)
                {
                    Console.WriteLine(
                        "[RECORD] Force killing FFmpeg...");
                    _ffmpegProcess.Kill();
                    _ffmpegProcess.WaitForExit(3000);
                }

                _ffmpegProcess.Dispose();
                _ffmpegProcess = null;

                Thread.Sleep(500); // Wait for file write

                if (File.Exists(_outputPath))
                {
                    var info = new FileInfo(_outputPath);
                    Console.WriteLine(
                        $"[RECORD] ✅ Saved → {_outputPath} " +
                        $"({info.Length / 1024}KB)");
                    return _outputPath;
                }
                else
                {
                    Console.WriteLine("[RECORD] ⚠ File not found!");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[RECORD] ⚠ Stop error: {ex.Message}");
                return null;
            }
        }
        public static string? ConvertToGif(string? mp4Path)
        {
            if (mp4Path == null || !File.Exists(mp4Path))
                return null;

            try
            {
                string gifPath = mp4Path.Replace(".mp4", ".gif");

                // Take only first 30 seconds for GIF
                var startInfo = new ProcessStartInfo
                {
                    FileName              = "ffmpeg",
                    Arguments             =
                        $"-y -i \"{mp4Path}\" " +
                        $"-t 30 " +                    // First 30 sec only
                        $"-vf \"fps=5,scale=800:-1:flags=lanczos," +
                        $"split[s0][s1];[s0]palettegen[p];" +
                        $"[s1][p]paletteuse\" " +
                        $"\"{gifPath}\"",
                    UseShellExecute       = false,
                    CreateNoWindow        = true,
                    RedirectStandardError = true
                };

                var proc = Process.Start(startInfo);
                proc?.BeginErrorReadLine();
                proc?.WaitForExit(120000);

                if (File.Exists(gifPath) &&
                    new FileInfo(gifPath).Length > 0)
                {
                    var info = new FileInfo(gifPath);
                    Console.WriteLine(
                        $"[RECORD] 🎞 GIF → {gifPath} " +
                        $"({info.Length / 1024}KB)");
                    return gifPath;
                }
                else
                {
                    Console.WriteLine(
                        "[RECORD] ⚠ GIF failed — MP4 use pannalam");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RECORD] ⚠ GIF error: {ex.Message}");
            }
            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Xml.XPath;

namespace FDK
{
    // JDG DOCO!
    public static class LoudnessMetadataLoader
    {
        private const string Bs1770GainExeFileName = "bs1770gain.exe";

        private static readonly Stack<string> Jobs = new Stack<string>();
        private static readonly object LockObject = new object();

        private static Thread ScanningThread;
        private static Semaphore Semaphore;

        // JDG Need to stop and start around song play.
        public static void StartBackgroundScanning()
        {
            // JDG Make background scanning conditional on configuration

            if (!IsBs1770GainAvailable())
            {
                return;
            }

            lock (LockObject)
            {
                Semaphore = new Semaphore(Jobs.Count, int.MaxValue);
                ScanningThread = new Thread(Scan)
                {
                    IsBackground = true,
                    Name = "LoudnessMetadataLoader background scanning thread.",
                    Priority = ThreadPriority.Lowest
                };
                ScanningThread.Start();
            }
        }

        public static void StopBackgroundScanning(bool joinImmediately)
        {
            var scanningThread = ScanningThread;

            // JDG Clean up the start and stop, especially wrt detection of bs1770gain
            if (scanningThread == null)
            {
                return;
            }

            ScanningThread = null;
            Semaphore.Release();
            Semaphore = null;

            if (joinImmediately)
            {
                scanningThread.Join();
            }
        }

        public static LoudnessMetadata? LoadForAudioPath(string absoluteBgmPath)
        {
            var loudnessMetadataPath = GetLoudnessMetadataPath(absoluteBgmPath);

            if (!File.Exists(loudnessMetadataPath))
            {
                Push(absoluteBgmPath);

                return null;
            }

            return LoadFromMetadataPath(loudnessMetadataPath);
        }

        private static string GetLoudnessMetadataPath(string absoluteBgmPath)
        {
            return Path.Combine(
                Path.GetDirectoryName(absoluteBgmPath),
                Path.GetFileNameWithoutExtension(absoluteBgmPath) + ".bs1770gain.xml");
        }

        private static LoudnessMetadata? LoadFromMetadataPath(string loudnessMetadataPath)
        {
            var xPathDocument = new XPathDocument(loudnessMetadataPath);

            var trackNavigator = xPathDocument.CreateNavigator()
                .SelectSingleNode(@"//bs1770gain/album/track[@total=""1"" and @number=""1""]");

            // JDG Layer in good error handling, especially for parsing cases like Dummy (opening and closing root element but nothing else)

            var integrated = trackNavigator
                .SelectSingleNode(@"integrated/@lufs").ValueAsDouble;

            var truePeak = trackNavigator
                .SelectSingleNode(@"true-peak/@tpfs").ValueAsDouble;

            return new LoudnessMetadata(new Lufs(integrated), new Lufs(truePeak));
        }

        private static void Push(string absoluteBgmPath)
        {
            lock (LockObject)
            {
                // Quite often, the loading process will cause the same job to be enqueued many times.
                // As such, we'll do a quick check as when this happens an equivalent job will often
                // already be at the top of the stack and we need not add it again.
                //
                // Note that we will not scan the whole stack as that is an O(n) operation on the main
                // thread, whereas redundant file existence checks on the background thread are not harmful.
                //
                // We also do not want to scan the whole stack because we want to re-queue jobs as the
                // user interacts with their data, usually by scrolling through songs and previewing them.
                if (Jobs.Count == 0 || Jobs.Peek() != absoluteBgmPath)
                {
                    Jobs.Push(absoluteBgmPath);
                    Semaphore.Release();
                }
            }
        }

        private static void Scan()
        {
            while (ScanningThread != null)
            {
                Semaphore?.WaitOne();

                if (ScanningThread == null)
                {
                    return;
                }

                int jobCount;
                string absoluteBgmPath;
                lock (LockObject)
                {
                    jobCount = Jobs.Count;
                    absoluteBgmPath = Jobs.Pop();
                }

                if (!File.Exists(absoluteBgmPath))
                {
                    Console.WriteLine($"JDG Scanning jobs outstanding: {jobCount - 1}. Missing audio file. Skipping {absoluteBgmPath}...");
                    continue;
                }

                var loudnessMetadataPath = GetLoudnessMetadataPath(absoluteBgmPath);

                if (File.Exists(loudnessMetadataPath))
                {
                    Console.WriteLine($"JDG Scanning jobs outstanding: {jobCount - 1}. Pre-existing metadata. Skipping {absoluteBgmPath}...");
                    continue;
                }

                Console.WriteLine($"JDG Scanning jobs outstanding: {jobCount}. Scanning {absoluteBgmPath}...");
                var stopwatch = Stopwatch.StartNew();

                var arguments = $"-it --xml -f \"{Path.GetFileName(loudnessMetadataPath)}\" \"{Path.GetFileName(absoluteBgmPath)}\"";
                try
                {
                    File.Delete(loudnessMetadataPath);
                    Execute(Path.GetDirectoryName(absoluteBgmPath), Bs1770GainExeFileName, arguments, true);
                    var elapsed = stopwatch.Elapsed;
                    Console.WriteLine($"JDG Scanned in {elapsed.TotalSeconds}s. Estimated remaining: {elapsed.TotalSeconds * (jobCount - 1)}s.");
                }
                catch (Exception e) // JDG Remember to review the clipping cases now copied under devtestsongs/Loudness
                {
                    Console.WriteLine($"JDG Exception encountered scanning {absoluteBgmPath}");
                    Console.WriteLine(e); // JDG Integrate this temporary output with the standard logging for the app.
                }
            }
        }

        private static bool IsBs1770GainAvailable()
        {
            try
            {
                Execute(null, Bs1770GainExeFileName, "-h");
                return true;
            }
            catch (Win32Exception)
            {
                return false;
            }
            catch (Exception)
            {
                return false; // JDG Consider logging this one. Win32Exception is reasonably expected. This is not.
            }
        }

        private static string Execute(
            string workingDirectory, string fileName, string arguments, bool shouldFailOnStdErrDataReceived = false)
        {
            var processStartInfo = new ProcessStartInfo(fileName, arguments)
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = workingDirectory ?? ""
            };

            var stdout = new StringWriter();
            var stderr = new StringWriter();
            using (var process = Process.Start(processStartInfo))
            {
                process.OutputDataReceived += (s, e) =>
                {
                    if (e.Data != null)
                    {
                        stdout.Write(e.Data);
                        stdout.Write(Environment.NewLine);
                    }
                };
                var errorDataReceived = false;
                process.ErrorDataReceived += (s, e) =>
                {
                    if (e.Data != null)
                    {
                        errorDataReceived = true;
                        stderr.Write(e.Data);
                        stderr.Write(Environment.NewLine);
                    }
                };
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                if ((shouldFailOnStdErrDataReceived && errorDataReceived) || process.ExitCode != 0)
                {
                    var err = stderr.ToString();
                    if (string.IsNullOrEmpty(err)) err = stdout.ToString();
                    throw new Exception(
                        $"Execution of {processStartInfo.FileName} with arguments {processStartInfo.Arguments} failed with exit code {process.ExitCode}: {err}");
                }

                return stdout.ToString();
            }
        }
    }
}
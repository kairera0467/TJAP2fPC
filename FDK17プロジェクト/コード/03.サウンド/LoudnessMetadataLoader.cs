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

        public static LoudnessMetadata? Load(string absoluteBgmPath)
        {
            var metadataPath = GetLoudnessMetadataPath(absoluteBgmPath);

            if (!File.Exists(metadataPath))
            {
                Push(absoluteBgmPath);

                return null;
            }

            var xPathDocument = new XPathDocument(metadataPath);

            var trackNavigator = xPathDocument.CreateNavigator()
                .SelectSingleNode(@"//bs1770gain/album/track[@total=""1"" and @number=""1""]");

            // JDG Layer in good error handling

            var integrated = trackNavigator
                .SelectSingleNode(@"integrated/@lufs").ValueAsDouble;

            var truePeak = trackNavigator
                .SelectSingleNode(@"true-peak/@tpfs").ValueAsDouble;

            return new LoudnessMetadata(new Lufs(integrated), new Lufs(truePeak));
        }

        private static string GetLoudnessMetadataPath(string absoluteBgmPath)
        {
            return Path.Combine(
                Path.GetDirectoryName(absoluteBgmPath),
                Path.GetFileNameWithoutExtension(absoluteBgmPath) + ".bs1770gain.xml");
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

                string absoluteBgmPath;
                lock (LockObject)
                {
                    absoluteBgmPath = Jobs.Pop();
                }

                var arguments = $"-it --xml \"{Path.GetFileName(absoluteBgmPath)}\"";
                try
                {
                    var xml = Execute(Path.GetDirectoryName(absoluteBgmPath), Bs1770GainExeFileName, arguments, true);
                    File.WriteAllText(GetLoudnessMetadataPath(absoluteBgmPath), xml);
                }
                catch (Exception e)
                {
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
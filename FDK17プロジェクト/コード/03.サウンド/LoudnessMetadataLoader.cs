using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml.XPath;

namespace FDK
{
    // JDG DOCO!
    public static class LoudnessMetadataLoader
    {
        private static readonly Stack<string> Jobs = new Stack<string>();
        private static readonly object LockObject = new object();

        private static Semaphore Semaphore = null;
        private static Thread ScanningThread;

        // JDG Need to stop and start around song play.
        public static void StartBackgroundScanning()
        {
            // JDG Make background scanning conditional on configuration and availability of the binary

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
            ScanningThread = null;
            Semaphore.Release();

            if (joinImmediately)
            {
                scanningThread?.Join();
            }
        }

        public static LoudnessMetadata? Load(string absoluteBgmPath)
        {
            var metadataPath = Path.Combine(
                Path.GetDirectoryName(absoluteBgmPath),
                Path.GetFileNameWithoutExtension(absoluteBgmPath) + ".bs1770gain.xml");

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
                Semaphore.WaitOne();

                if (ScanningThread == null)
                {
                    return;
                }

                string absoluteBgmPath = null;
                lock (LockObject)
                {
                    absoluteBgmPath = Jobs.Pop();
                }

                // JDG For now we'll just sleep for a bit, to emulate processing.
                Thread.Sleep(2000);
            }
        }
    }
}
using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace TJAPlayer3.Tests
{
    [TestFixture]
    public sealed class CDTXStyleExtractorTests
    {
        private static readonly Encoding ShiftJisEncoding = Encoding.GetEncoding("Shift_JIS");

        [Test, Combinatorial]
        public void Test_tセッション譜面がある(
            [Values(
                "205 example",
                "205 example but with duplicate sheets",
                "expected case 1 and 2",
                "expected case 1 only",
                "expected case 2 only",
                "expected case couple only",
                "expected case double only",
                "expected case double only but with duplicate sheets",
                "expected case single and couple",
                "expected case single and double",
                "expected case single and double but with duplicate sheets",
                "expected case single only",
                "expected case single only whole file due to no course",
                "expected case single only whole file due to no course but with duplicate sheets",
                "expected case single only whole file due to no course but with no start",
                "kitchen sink couple only",
                "kitchen sink double only",
                "kitchen sink single and couple",
                "kitchen sink single and double",
                "kitchen sink single only",
                "mixed case double only",
                "mixed case single and double",
                "mixed case single only",
                "no course",
                "no style",
                "no style but with duplicate sheets",
                "trailing characters double only",
                "trailing characters single and double",
                "trailing characters single only")]
            string scenarioName,
            [Values(0, 1, 2)] int seqNo)
        {
            var assemblyPath = new Uri(GetType().Assembly.EscapedCodeBase).LocalPath;
            var assemblyDirectory = Path.GetDirectoryName(assemblyPath);
            var testDataDirectory = Path.Combine(Path.Combine(assemblyDirectory, "コード"), "スコア、曲");

            var scenarioFileNamePart = scenarioName.Replace(' ', '_');

            var inputDirectory = Path.Combine(testDataDirectory, "input");
            var inputFileName = $"{scenarioFileNamePart}.tja";
            var inputPath = Path.Combine(inputDirectory, inputFileName);
            var input = File.ReadAllText(inputPath, ShiftJisEncoding)
                .Replace("\r\n", "\n")
                .Replace('\t', ' ');

            var result = CDTXStyleExtractor.tセッション譜面がある(input, seqNo, inputPath);

            // I would use ApprovalTests.Net for this,
            // but cannot until we upgrade past .net 3.5.
            // Until then, this test will approximate it.

            var inputReferenceDirectory = Path.Combine(testDataDirectory, "inputReference");
            Directory.CreateDirectory(inputReferenceDirectory);
            var inputReferenceFileName = $"{scenarioFileNamePart}.{seqNo}.tja";
            var inputReferencePath = Path.Combine(inputReferenceDirectory, inputReferenceFileName);
            File.Delete(inputReferencePath);
            File.Copy(inputPath, inputReferencePath);
            
            var receivedDirectory = Path.Combine(testDataDirectory, "received");
            Directory.CreateDirectory(receivedDirectory);
            var receivedFileName = $"{scenarioFileNamePart}.{seqNo}.tja";
            var receivedPath = Path.Combine(receivedDirectory, receivedFileName);
            File.Delete(receivedPath);
            File.WriteAllText(receivedPath, result, ShiftJisEncoding);

            var approvedDirectory = Path.Combine(testDataDirectory, "approved");
            Directory.CreateDirectory(approvedDirectory);
            var approvedFileName = $"{scenarioFileNamePart}.{seqNo}.tja";
            var approvedPath = Path.Combine(approvedDirectory, approvedFileName);

            var approved = File.ReadAllText(approvedPath, ShiftJisEncoding).Replace("\r\n", "\n");
            var received = File.ReadAllText(receivedPath, ShiftJisEncoding);

            Assert.AreEqual(approved, received);
        }
    }
}
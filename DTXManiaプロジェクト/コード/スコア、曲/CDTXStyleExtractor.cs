using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DTXMania
{
    public static class CDTXStyleExtractor
    {
        private const RegexOptions CoursePlayRemovalRegexOptions =
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline;

        private static readonly Regex CourseDoublesPlayRemovalRegex =
            new Regex(@"^STYLE:(Double|Couple).*?$.*?^#START\sP1$.*?^#END$.*?^#START\sP2$.*?^#END$", CoursePlayRemovalRegexOptions);

        private static readonly Regex CourseSinglePlayRemovalRegex =
            new Regex(@"^STYLE:Single.*?$.*?^#END$", CoursePlayRemovalRegexOptions);

        private static readonly Regex CourseStartP1RemovalRegex =
            new Regex(@"^#START\sP1$.*?^#END$", CoursePlayRemovalRegexOptions);

        private static readonly Regex CourseStartP2RemovalRegex =
            new Regex(@"^#START\sP2$.*?^#END$", CoursePlayRemovalRegexOptions);

        /// <summary>
        /// Determine if there is a session notation, and if there is then
        /// return a sheet of music clipped according to the specified player Side.
        /// </summary>
        public static string tセッション譜面がある(string strTJA, int seqNo, string strファイル名の絶対パス)
        {
            void TraceError(string subMessage)
            {
                Trace.TraceError(FormatTraceMessage(subMessage));
            }

            void TraceWarning(string subMessage)
            {
                Trace.TraceWarning(FormatTraceMessage(subMessage));
            }

            string FormatTraceMessage(string subMessage)
            {
                return $"{nameof(CDTXStyleExtractor)} {subMessage} (seqNo={seqNo}, {strファイル名の絶対パス})";
            }

            //入力された譜面がnullでないかチェック。
            if (string.IsNullOrEmpty(strTJA))
            {
                TraceError("is returning false early due to null or empty strTJA.");
                return strTJA;
            }

            // Having no STYLE: means there can only be a single play chart.
            if (!strTJA.Contains("STYLE:"))
            {
                return strTJA;
            }

            switch (seqNo)
            {
                case 0:
                    break;
                case 1:
                case 2:
                {
                    var strDouble = CourseSinglePlayRemovalRegex.Replace(strTJA, "");

                    if (strDouble == strTJA)
                    {
                        TraceWarning("suspects incorrect STYLE:Single setup.");
                    }

                    if (!strDouble.Contains("STYLE:Double"))
                    {
                        TraceWarning("suspects incorrect STYLE:Double setup.");
                    }

                    var courseStartPOtherRemovalRegex = seqNo == 1 ? CourseStartP2RemovalRegex : CourseStartP1RemovalRegex;
                    var pOther = seqNo == 1 ? 2 : 1;

                    var strDoublePx = courseStartPOtherRemovalRegex.Replace(strDouble, "");

                    if (strDoublePx == strDouble)
                    {
                        TraceWarning($"suspects incorrect STYLE:Double #START P{pOther} setup.");
                    }

                    if (strDoublePx.Contains($"#START P{seqNo}"))
                    {
                        return strDoublePx;
                    }

                    TraceWarning($"suspects incorrect STYLE:Double #START P{seqNo} setup. Attempting to use the single player course.");
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(seqNo), seqNo, "Expected 0, 1, or 2.");
            }

            var strSingle = CourseDoublesPlayRemovalRegex.Replace(strTJA, "");

            if (strSingle == strTJA)
            {
                TraceWarning("suspects incorrect STYLE:Double setup.");
            }

            if (!strSingle.Contains("STYLE:Single"))
            {
                TraceWarning("suspects incorrect STYLE:Single setup.");
            }

            return strSingle;
        }
    }
}
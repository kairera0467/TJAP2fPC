using System.IO;
using System.Xml.XPath;

namespace FDK
{
    // JDG DOCO!
    public static class LoudnessMetadataLoader
    {
        public static LoudnessMetadata? Load(string absoluteBgmPath)
        {
            var metadataPath = Path.Combine(
                Path.GetDirectoryName(absoluteBgmPath),
                Path.GetFileNameWithoutExtension(absoluteBgmPath) + ".bs1770gain.xml");

            if (!File.Exists(metadataPath))
            {
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
    }
}
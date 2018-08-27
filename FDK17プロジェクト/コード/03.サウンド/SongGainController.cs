using System;

namespace FDK
{
    // JDG Update SongGainController doco for loudness metadata support
    /// <summary>
    /// SongGainController provides a central place through which song preview
    /// and song playback attempt to apply SONGVOL as the Gain of a song sound.
    ///
    /// By doing so through SongGainController instead of directly against the
    /// song (preview) CSound object, SongGainController can override the Gain
    /// value based on configuration or other information.
    /// </summary>
    public sealed class SongGainController
    {
        public bool ApplyLoudnessMetadata { private get; set; }
        public Lufs TargetLoudness { private get; set; }
        public bool ApplySongVol { private get; set; }

        public void Set(int songVol, LoudnessMetadata? songLoudnessMetadata, CSound sound)
        {
            if (ApplyLoudnessMetadata && songLoudnessMetadata.HasValue)
            {
                // JDG Also for now, we're going to hack the gain value into place right here and now
                // JDG and will flow it through further in a later revision.
                var dbGain = TargetLoudness.ToDouble() - songLoudnessMetadata.Value.Integrated.ToDouble();

                // JDG Once more logic moves to CSound, safe gain can account for the other mixed values
                var safeTruePeakDbGain = 0.0 - songLoudnessMetadata.Value.TruePeak?.ToDouble() ?? 0.0;
                var safeDbGain = dbGain < safeTruePeakDbGain ? dbGain : safeTruePeakDbGain;

                var gainMultiplier = Math.Pow(10, safeDbGain / 20.0);
                var gain = gainMultiplier * 100.0;
                sound.Gain = (int)Math.Round(gain);
            }
            else
            {
                sound.Gain = ApplySongVol ? songVol : CSound.DefaultSongVol;
            }
        }
    }
}

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
                var dbGain = TargetLoudness.ToDouble() - songLoudnessMetadata.Value.Integrated.ToDouble();

                sound.SetGain(new Lufs(dbGain), songLoudnessMetadata.Value.TruePeak);
            }
            else
            {
                sound.SetGain(ApplySongVol ? songVol : CSound.DefaultSongVol);
            }
        }
    }
}

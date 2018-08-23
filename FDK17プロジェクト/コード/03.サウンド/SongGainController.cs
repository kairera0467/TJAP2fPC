namespace FDK
{
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
        public bool ApplySongVol { get; set; }

        public void Set(int songVol, CSound sound)
        {
            sound.Gain = ApplySongVol ? songVol : CSound.DefaultSongVol;
        }
    }
}

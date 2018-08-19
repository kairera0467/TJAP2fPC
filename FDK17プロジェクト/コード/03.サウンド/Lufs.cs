using System;

namespace FDK
{
    /// <summary>
    /// The Lufs structure is used to carry, and assist with calculations related to,
    /// Loudness Units relative to Full Scale. LUFS are measured in absolute scale
    /// and whole values represent one decibel.
    /// </summary>
    [Serializable]
    public struct Lufs
    {
        private readonly double _value;

        public Lufs(double value)
        {
            _value = value;
        }

        public double ToDouble() => _value;
    }
}

using System;

namespace Hytera.EEMS.Media
{
    public sealed class VlcMediaPlayerScrambledChangedEventArgs : EventArgs
    {
        public VlcMediaPlayerScrambledChangedEventArgs(int newScrambled)
        {
            NewScrambled = newScrambled;
        }

        public int NewScrambled { get; private set; }
    }
}
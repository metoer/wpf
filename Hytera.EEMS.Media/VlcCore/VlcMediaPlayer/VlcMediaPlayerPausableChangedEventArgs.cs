using System;

namespace Hytera.EEMS.Media
{
    public sealed class VlcMediaPlayerPausableChangedEventArgs : EventArgs
    {
        public VlcMediaPlayerPausableChangedEventArgs(bool paused)
        {
            IsPaused = paused;
        }

        public bool IsPaused { get; private set; }
    }
}
using System;

namespace Hytera.EEMS.Media
{
    public sealed class VlcMediaPlayerVideoOutChangedEventArgs : EventArgs
    {
        public VlcMediaPlayerVideoOutChangedEventArgs(int newCount)
        {
            NewCount = newCount;
        }

        public int NewCount { get; private set; }
    }
}
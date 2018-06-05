using System;

namespace Hytera.EEMS.Media
{
    public sealed class VlcMediaPlayerMediaChangedEventArgs : EventArgs
    {
        public VlcMediaPlayerMediaChangedEventArgs(VlcMedia newMedia)
        {
            NewMedia = newMedia;
        }

        public VlcMedia NewMedia { get; private set; }
    }
}
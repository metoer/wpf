using System;

namespace Hytera.EEMS.Media
{
    public class VlcMediaDurationChangedEventArgs : EventArgs
    {
        public VlcMediaDurationChangedEventArgs(long newDuration)
        {
            NewDuration = newDuration;
        }

        public long NewDuration { get; private set; }
    }
}
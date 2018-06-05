using System;

namespace Hytera.EEMS.Media
{
    public class VlcMediaParsedChangedEventArgs : EventArgs
    {
        public VlcMediaParsedChangedEventArgs(int newStatus)
        {
            NewStatus = newStatus;
        }

        public int NewStatus { get; private set; }
    }
}
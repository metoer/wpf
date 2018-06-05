using Hytera.EEMS.Media.Signatures;
using System;

namespace Hytera.EEMS.Media
{
    public class VlcMediaStateChangedEventArgs : EventArgs
    {
        public VlcMediaStateChangedEventArgs(MediaStates state)
        {
            State = state;
        }

        public MediaStates State { get; private set; }
    }
}
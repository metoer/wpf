using System;

namespace Hytera.EEMS.Media
{
    public class VlcMediaSubItemTreeAddedEventArgs : EventArgs
    {
        public VlcMediaSubItemTreeAddedEventArgs(VlcMedia subItemTreeAdded)
        {
            SubItemTreeAdded = subItemTreeAdded;
        }

        public VlcMedia SubItemTreeAdded { get; private set; }
    }
}
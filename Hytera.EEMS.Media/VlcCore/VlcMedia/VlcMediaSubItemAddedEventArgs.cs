using System;

namespace Hytera.EEMS.Media
{
    public class VlcMediaSubItemAddedEventArgs : EventArgs
    {
        public VlcMediaSubItemAddedEventArgs(VlcMedia subItemAdded)
        {
            SubItemAdded = subItemAdded;
        }

        public VlcMedia SubItemAdded { get; private set; }
    }
}
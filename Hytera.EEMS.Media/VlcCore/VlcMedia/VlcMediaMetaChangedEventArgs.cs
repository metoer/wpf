using Hytera.EEMS.Media.Signatures;
using System;

namespace Hytera.EEMS.Media
{
    public class VlcMediaMetaChangedEventArgs : EventArgs
    {
        public VlcMediaMetaChangedEventArgs(MediaMetadatas metaType)
        {
            MetaType = metaType;
        }

        public MediaMetadatas MetaType { get; private set; }
    }
}
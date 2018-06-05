using System;

namespace Hytera.EEMS.Media
{
    public sealed class VlcMediaPlayerSnapshotTakenEventArgs : EventArgs
    {
        public VlcMediaPlayerSnapshotTakenEventArgs(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; private set; }
    }
}
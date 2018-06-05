using System;
using System.IO;

namespace Hytera.EEMS.Media.Controls
{
    public sealed class VlcLibDirectoryNeededEventArgs : EventArgs
    {
        public DirectoryInfo VlcLibDirectory { get; set; }

        public VlcLibDirectoryNeededEventArgs()
        {
            
        }
    }
}
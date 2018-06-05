using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TrackDescriptionStructure
    {
        public int Id;
        public string Name;
        public IntPtr NextTrackDescription;
    }
}

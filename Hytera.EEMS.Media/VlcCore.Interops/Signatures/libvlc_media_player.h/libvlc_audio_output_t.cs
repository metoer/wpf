using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AudioOutputDescriptionStructure
    {
        public string Name;
        public string Description;
        public IntPtr NextAudioOutputDescription;
    }
}

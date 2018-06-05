using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle
    {
        public int Top;
        public int Left;
        public int Bottom;
        public int Right;
    }
}

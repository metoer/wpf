using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Get current teletext page requested.
    /// </summary>
    [LibVlcFunction("libvlc_video_get_teletext")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int GetVideoTeletext(IntPtr mediaPlayerInstance);
}

using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Get a string marquee option value.
    /// </summary>
    [LibVlcFunction("libvlc_video_get_marquee_string")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr GetVideoMarqueeString(IntPtr mediaPlayerInstance, VideoMarqueeOptions option);
}

using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Enable or disable deinterlace filter.
    /// </summary>
    [LibVlcFunction("libvlc_video_set_deinterlace")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void SetVideoDeinterlace(IntPtr mediaPlayerInstance, IntPtr mode);
}

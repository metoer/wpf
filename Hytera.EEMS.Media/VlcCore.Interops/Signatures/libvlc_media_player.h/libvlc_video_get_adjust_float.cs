using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Get an float adjust option value.
    /// </summary>
    [LibVlcFunction("libvlc_video_get_adjust_float")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate float GetVideoAdjustFloat(IntPtr mediaPlayerInstance, VideoAdjustOptions option);
}

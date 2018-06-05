using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Set adjust option as float.
    /// </summary>
    [LibVlcFunction("libvlc_video_set_adjust_float")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void SetVideoAdjustFloat(IntPtr mediaPlayerInstance, VideoAdjustOptions option, float value);
}

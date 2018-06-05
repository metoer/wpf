using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Set logo option as string.
    /// </summary>
    [LibVlcFunction("libvlc_video_set_logo_string")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void SetVideoLogoString(IntPtr mediaPlayerInstance, VideoLogoOptions option, IntPtr value);
}

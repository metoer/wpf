﻿using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Set a string marquee option value.
    /// </summary>
    [LibVlcFunction("libvlc_video_set_marquee_string")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void SetVideoMarqueeString(IntPtr mediaPlayerInstance, VideoMarqueeOptions option, IntPtr value);
}
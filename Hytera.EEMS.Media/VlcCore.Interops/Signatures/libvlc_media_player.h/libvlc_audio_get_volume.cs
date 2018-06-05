﻿using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Get current software audio volume.
    /// </summary>
    [LibVlcFunction("libvlc_audio_get_volume")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int GetVolume(IntPtr mediaPlayerInstance);
}

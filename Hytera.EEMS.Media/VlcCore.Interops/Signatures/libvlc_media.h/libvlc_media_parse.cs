﻿using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Parse a media meta data and tracks information. 
    /// </summary>
    [LibVlcFunction("libvlc_media_parse")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void ParseMedia(IntPtr mediaInstance);
}

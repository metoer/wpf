﻿using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Set next media chapter (if applicable).
    /// </summary>
    [LibVlcFunction("libvlc_media_player_next_chapter")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void SetNextMediaChapter(IntPtr mediaPlayerInstance);
}

using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Get media is seekable.
    /// </summary>
    [LibVlcFunction("libvlc_media_player_is_seekable")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int IsSeekable(IntPtr mediaPlayerInstance);
}

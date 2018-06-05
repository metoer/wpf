using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Get media position.
    /// </summary>
    [LibVlcFunction("libvlc_media_player_get_position")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate float GetMediaPosition(IntPtr mediaPlayerInstance);
}

using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Get Rate at which movie is playing.
    /// </summary>
    /// <returns>Get the requested movie play rate.</returns>
    [LibVlcFunction("libvlc_media_player_get_time")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate long GetTime(IntPtr mediaPlayerInstance);
}

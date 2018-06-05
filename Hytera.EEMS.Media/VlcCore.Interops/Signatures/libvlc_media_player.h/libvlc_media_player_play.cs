using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// play
    /// </summary>
    /// <param name="mediaPlayerInstance"></param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_media_player_play")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int Play(IntPtr mediaPlayerInstance);
}

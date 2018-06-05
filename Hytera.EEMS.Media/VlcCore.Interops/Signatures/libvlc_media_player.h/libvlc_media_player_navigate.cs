using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Navigate through DVD Menu.
    /// </summary>
    [LibVlcFunction("libvlc_media_player_navigate")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void Navigate(IntPtr mediaPlayerInstance, NavigateModes navigate);
}

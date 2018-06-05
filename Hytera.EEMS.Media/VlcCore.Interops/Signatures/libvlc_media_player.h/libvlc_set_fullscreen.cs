using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// SetFullScreen.
    /// </summary>
    /// <returns>Return 0 if playback started (and was already started), or -1 on error.</returns>
    [LibVlcFunction("libvlc_set_fullscreen")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int SetFullScreen(IntPtr mediaPlayerInstance, int isFullScreen);
}

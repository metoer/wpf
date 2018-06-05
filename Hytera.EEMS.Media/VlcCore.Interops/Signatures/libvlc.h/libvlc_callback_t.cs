using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{

    /// <summary>
    /// CCallback function notification.
    /// </summary>
    [LibVlcFunction("libvlc_callback_t")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void EventCallback(IntPtr args);
}

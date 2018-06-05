using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Frees an heap allocation returned by a LibVLC function.
    /// </summary>
    /// <returns>Return the libvlc instance or NULL in case of error.</returns>
    [LibVlcFunction("libvlc_free")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void Free(IntPtr ptr);
}

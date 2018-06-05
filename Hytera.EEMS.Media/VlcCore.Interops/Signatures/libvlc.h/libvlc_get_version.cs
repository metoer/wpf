using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Retrieve libvlc version.
    /// </summary>
    /// <returns>Return a string containing the libvlc version.</returns>
    [LibVlcFunction("libvlc_get_version")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr GetVersion();
}

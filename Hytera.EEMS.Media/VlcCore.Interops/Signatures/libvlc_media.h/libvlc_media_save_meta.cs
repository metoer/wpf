using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Save the meta previously set.
    /// </summary>
    [LibVlcFunction("libvlc_media_save_meta")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int SaveMediaMetadatas(IntPtr mediaInstance);
}

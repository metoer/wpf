using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Get Parsed status for media descriptor object.
    /// </summary>
    [LibVlcFunction("libvlc_media_is_parsed")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int IsParsedMedia(IntPtr mediaInstance);
}

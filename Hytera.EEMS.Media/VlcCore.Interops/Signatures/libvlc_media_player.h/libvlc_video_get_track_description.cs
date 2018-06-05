using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Get the description of available video tracks.
    /// </summary>
    [LibVlcFunction("libvlc_video_get_track_description")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate TrackDescriptionStructure GetVideoTracksDescriptions(IntPtr mediaPlayerInstance);
}

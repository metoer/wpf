using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Set current audio delay. The audio delay will be reset to zero each time the media changes.
    /// </summary>
    [LibVlcFunction("libvlc_audio_set_delay")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void SetAudioDelay(IntPtr mediaPlayerInstance, long channel);
}

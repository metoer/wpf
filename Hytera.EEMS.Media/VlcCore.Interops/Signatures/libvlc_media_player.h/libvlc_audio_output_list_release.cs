using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// It will release the list of available audio outputs.
    /// </summary>
    [LibVlcFunction("libvlc_audio_output_list_release")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void ReleaseAudioOutputDescription(AudioOutputDescriptionStructure audioOutput);
}

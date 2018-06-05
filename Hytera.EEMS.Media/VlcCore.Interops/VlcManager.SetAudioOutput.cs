using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public void SetAudioOutput(AudioOutputDescriptionStructure output)
        {
            SetAudioOutput(output.Name);
        }

        public void SetAudioOutput(string outputName)
        {
#if NET20
            GetInteropDelegate<SetAudioOutput>().Invoke(myVlcInstance, StringExtensions.ToHGlobalAnsi(outputName));
#else
            GetInteropDelegate<SetAudioOutput>().Invoke(myVlcInstance, outputName.ToHGlobalAnsi());
#endif
        }
    }
}

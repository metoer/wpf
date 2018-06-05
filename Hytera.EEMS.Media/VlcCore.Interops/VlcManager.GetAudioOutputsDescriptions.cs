using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public AudioOutputDescriptionStructure GetAudioOutputsDescriptions()
        {
            return GetInteropDelegate<GetAudioOutputsDescriptions>().Invoke(myVlcInstance);
        }
    }
}

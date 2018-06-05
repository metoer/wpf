using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public string GetAudioOutputDeviceLongName(string audioOutputDescriptionName, int deviceIndex)
        {
            return IntPtrExtensions.ToStringAnsi(GetInteropDelegate<GetAudioOutputDeviceLongName>().Invoke(myVlcInstance, StringExtensions.ToHGlobalAnsi(audioOutputDescriptionName), deviceIndex));
        }
    }
}

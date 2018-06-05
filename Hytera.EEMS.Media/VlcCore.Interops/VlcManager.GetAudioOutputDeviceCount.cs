using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public int GetAudioOutputDeviceCount(string outputName)
        {
            return GetInteropDelegate<GetAudioOutputDeviceCount>().Invoke(myVlcInstance, outputName);
        }
    }
}

using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public string GetAudioOutputDeviceName(string audioOutputDescriptionName, int deviceIndex)
        {
            return IntPtrExtensions.ToStringAnsi(GetInteropDelegate<GetAudioOutputDeviceName>().Invoke(myVlcInstance, StringExtensions.ToHGlobalAnsi(audioOutputDescriptionName), deviceIndex));
        }
    }
}

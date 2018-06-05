using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public void ReleaseAudioOutputDescription(AudioOutputDescriptionStructure description)
        {
            GetInteropDelegate<ReleaseAudioOutputDescription>().Invoke(description);
        }
    }
}

using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public void SetAudioChannel(VlcMediaPlayerInstance mediaPlayerInstance, int channel)
        {
            if (mediaPlayerInstance == IntPtr.Zero)
                throw new ArgumentException("Media player instance is not initialized.");
            GetInteropDelegate<SetAudioChannel>().Invoke(mediaPlayerInstance, channel);
        }
    }
}

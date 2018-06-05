using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public VlcMediaInstance GetMediaFromMediaPlayer(VlcMediaPlayerInstance mediaPlayerInstance)
        {
            if (mediaPlayerInstance == IntPtr.Zero)
                throw new ArgumentException("Media player instance is not initialized.");
            return VlcMediaInstance.New(this, GetInteropDelegate<GetMediaFromMediaPlayer>().Invoke(mediaPlayerInstance));
        }
    }
}

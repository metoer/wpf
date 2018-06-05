using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public VlcMediaPlayerEventManagerInstance GetMediaPlayerEventManager(VlcMediaPlayerInstance mediaPlayerInstance)
        {
            if (mediaPlayerInstance == IntPtr.Zero)
                throw new ArgumentException("Media player instance is not initialized.");
            return new VlcMediaPlayerEventManagerInstance(this, GetInteropDelegate<GetMediaPlayerEventManager>().Invoke(mediaPlayerInstance));
        }
    }
}

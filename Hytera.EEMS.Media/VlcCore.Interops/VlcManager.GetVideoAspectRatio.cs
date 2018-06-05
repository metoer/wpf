using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public string GetVideoAspectRatio(VlcMediaPlayerInstance mediaPlayerInstance)
        {
            if (mediaPlayerInstance == IntPtr.Zero)
                throw new ArgumentException("Media player instance is not initialized.");
#if NET20
            return IntPtrExtensions.ToStringAnsi(GetInteropDelegate<GetVideoAspectRatio>().Invoke(mediaPlayerInstance));
#else
            return GetInteropDelegate<GetVideoAspectRatio>().Invoke(mediaPlayerInstance).ToStringAnsi();
#endif
        }
    }
}

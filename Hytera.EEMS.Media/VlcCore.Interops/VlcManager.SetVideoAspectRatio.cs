using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public void SetVideoAspectRatio(VlcMediaPlayerInstance mediaPlayerInstance, string cropGeometry)
        {
            if (mediaPlayerInstance == IntPtr.Zero)
                throw new ArgumentException("Media player instance is not initialized.");
#if NET20
            GetInteropDelegate<SetVideoAspectRatio>().Invoke(mediaPlayerInstance, StringExtensions.ToHGlobalAnsi(cropGeometry));
#else
            GetInteropDelegate<SetVideoAspectRatio>().Invoke(mediaPlayerInstance, cropGeometry.ToHGlobalAnsi());
#endif
        }
    }
}

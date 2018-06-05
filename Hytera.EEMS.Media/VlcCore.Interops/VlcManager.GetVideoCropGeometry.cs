using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public string GetVideoCropGeometry(VlcMediaPlayerInstance mediaPlayerInstance)
        {
            if (mediaPlayerInstance == IntPtr.Zero)
                throw new ArgumentException("Media player instance is not initialized.");
#if NET20
            return IntPtrExtensions.ToStringAnsi(GetInteropDelegate<GetVideoCropGeometry>().Invoke(mediaPlayerInstance));
#else
            return GetInteropDelegate<GetVideoCropGeometry>().Invoke(mediaPlayerInstance).ToStringAnsi();
#endif
        }
    }
}

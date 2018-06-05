using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public void SetVideoDeinterlace(VlcMediaPlayerInstance mediaPlayerInstance, string deinterlaceMode)
        {
            if (mediaPlayerInstance == IntPtr.Zero)
                throw new ArgumentException("Media player instance is not initialized.");
#if NET20
            GetInteropDelegate<SetVideoDeinterlace>().Invoke(mediaPlayerInstance, StringExtensions.ToHGlobalAnsi(deinterlaceMode));
#else
            GetInteropDelegate<SetVideoDeinterlace>().Invoke(mediaPlayerInstance, deinterlaceMode.ToHGlobalAnsi());
#endif
        }
    }
}

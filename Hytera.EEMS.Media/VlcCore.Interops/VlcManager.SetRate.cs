using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public void SetRate(VlcMediaPlayerInstance mediaPlayerInstance, float rate)
        {
            if (mediaPlayerInstance == IntPtr.Zero)
                throw new ArgumentException("Media player instance is not initialized.");
            GetInteropDelegate<SetRate>().Invoke(mediaPlayerInstance, rate);
        }
    }
}

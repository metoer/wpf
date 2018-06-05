using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public void SetMediaPosition(VlcMediaPlayerInstance mediaPlayerInstance, float position)
        {
            if (mediaPlayerInstance == IntPtr.Zero)
                throw new ArgumentException("Media player instance is not initialized.");
            GetInteropDelegate<SetMediaPosition>().Invoke(mediaPlayerInstance, position);
        }
    }
}

using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public void ReleaseMediaPlayer(VlcMediaPlayerInstance mediaPlayerInstance)
        {
            if (mediaPlayerInstance == IntPtr.Zero)
                return;
            try
            {
                GetInteropDelegate<ReleaseMediaPlayer>().Invoke(mediaPlayerInstance);
            }
            finally
            {
                mediaPlayerInstance.Pointer = IntPtr.Zero;
            }
        }
    }
}

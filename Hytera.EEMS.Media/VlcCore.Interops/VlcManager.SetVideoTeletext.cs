using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public void SetVideoTeletext(VlcMediaPlayerInstance mediaPlayerInstance, int teletextPage)
        {
            if (mediaPlayerInstance == IntPtr.Zero)
                throw new ArgumentException("Media player instance is not initialized.");
            GetInteropDelegate<SetVideoTeletext>().Invoke(mediaPlayerInstance, teletextPage);
        }
    }
}

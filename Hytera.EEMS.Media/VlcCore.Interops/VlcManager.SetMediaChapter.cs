using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public void SetMediaChapter(VlcMediaPlayerInstance mediaPlayerInstance, int chapter)
        {
            if (mediaPlayerInstance == IntPtr.Zero)
                throw new ArgumentException("Media player instance is not initialized.");
            GetInteropDelegate<SetMediaChapter>().Invoke(mediaPlayerInstance, chapter);
        }
    }
}

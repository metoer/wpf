using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public void SetMediaFullScreen(VlcMediaPlayerInstance mediaPlayerInstance, bool isFull)
        {
            if (mediaPlayerInstance == IntPtr.Zero)
                throw new ArgumentException("Media player instance is not initialized.");
            GetInteropDelegate<SetFullScreen>().Invoke(mediaPlayerInstance, isFull ? 1 : 0);
        }
    }
}

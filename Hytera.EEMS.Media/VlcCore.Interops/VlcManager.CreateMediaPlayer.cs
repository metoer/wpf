using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public VlcMediaPlayerInstance CreateMediaPlayer()
        {
            return new VlcMediaPlayerInstance(this, GetInteropDelegate<CreateMediaPlayer>().Invoke(myVlcInstance));
        }
    }
}

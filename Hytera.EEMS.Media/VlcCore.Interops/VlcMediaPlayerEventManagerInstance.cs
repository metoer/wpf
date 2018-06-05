using System;

namespace Hytera.EEMS.Media
{
    public sealed class VlcMediaPlayerEventManagerInstance : VlcEventManagerInstance
    {
        internal VlcMediaPlayerEventManagerInstance(VlcManager manager, IntPtr pointer)
            : base(manager, pointer)
        {
        }
    }
}
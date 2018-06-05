using System;

namespace Hytera.EEMS.Media
{
    public sealed class VlcMediaEventManagerInstance : VlcEventManagerInstance
    {
        internal VlcMediaEventManagerInstance(VlcManager manager, IntPtr pointer)
            : base(manager, pointer)
        {
        }
    }
}
using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public string GetEventTypeName(EventTypes eventType)
        {
#if !NET20
            return GetInteropDelegate<GetEventTypeName>().Invoke(eventType).ToStringAnsi();
#else
            return IntPtrExtensions.ToStringAnsi(GetInteropDelegate<GetEventTypeName>().Invoke(eventType));
#endif
        }
    }
}

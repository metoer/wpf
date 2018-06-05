using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public string GetLastErrorMessage()
        {
#if !NET20
            return GetInteropDelegate<GetLastErrorMessage>().Invoke().ToStringAnsi();
#else
            return IntPtrExtensions.ToStringAnsi(GetInteropDelegate<GetLastErrorMessage>().Invoke());
#endif
        }
    }
}

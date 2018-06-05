using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public string GetChangeset()
        {
#if !NET20
            return GetInteropDelegate<GetChangeset>().Invoke().ToStringAnsi();
#else
            return IntPtrExtensions.ToStringAnsi(GetInteropDelegate<GetChangeset>().Invoke());
#endif
        }
    }
}

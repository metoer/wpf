using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public string GetCompiler()
        {
#if !NET20
            return GetInteropDelegate<GetCompiler>().Invoke().ToStringAnsi();
#else
            return IntPtrExtensions.ToStringAnsi(GetInteropDelegate<GetCompiler>().Invoke());
#endif
        }
    }
}

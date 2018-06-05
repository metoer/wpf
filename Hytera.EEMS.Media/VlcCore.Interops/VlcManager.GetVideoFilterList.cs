using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public IntPtr GetVideoFilterList()
        {
            return GetInteropDelegate<GetVideoFilterList>().Invoke(myVlcInstance);
        }
    }
}

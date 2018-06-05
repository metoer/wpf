using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public IntPtr GetAudioFilterList()
        {
            return GetInteropDelegate<GetAudioFilterList>().Invoke(myVlcInstance);
        }
    }
}

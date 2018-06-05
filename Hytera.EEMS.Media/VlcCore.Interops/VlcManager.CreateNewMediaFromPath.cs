using System.Runtime.InteropServices;
using System.Text;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public VlcMediaInstance CreateNewMediaFromPath(string mrl)
        {
            var handle = GCHandle.Alloc(Encoding.UTF8.GetBytes(mrl), GCHandleType.Pinned);
            var result = VlcMediaInstance.New(this, GetInteropDelegate<CreateNewMediaFromPath>().Invoke(myVlcInstance, handle.AddrOfPinnedObject()));
            handle.Free();
            return result;
        }
    }
}

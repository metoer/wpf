using System;
using System.Runtime.InteropServices;
using System.Text;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public void AddOptionFlagToMedia(VlcMediaInstance mediaInstance, string option, uint flag)
        {
            if (mediaInstance == IntPtr.Zero)
                throw new ArgumentException("Media instance is not initialized.");
            var handle = GCHandle.Alloc(Encoding.UTF8.GetBytes(option), GCHandleType.Pinned);
            GetInteropDelegate<AddOptionFlagToMedia>().Invoke(mediaInstance, handle.AddrOfPinnedObject(), flag);
            handle.Free();
        }
    }
}

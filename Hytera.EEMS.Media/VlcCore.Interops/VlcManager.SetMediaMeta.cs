using System;
using System.Runtime.InteropServices;
using System.Text;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public void SetMediaMeta(VlcMediaInstance mediaInstance, MediaMetadatas metadata, string value)
        {
            if (mediaInstance == IntPtr.Zero)
                throw new ArgumentException("Media instance is not initialized.");
            var handle = GCHandle.Alloc(Encoding.UTF8.GetBytes(value), GCHandleType.Pinned);
            GetInteropDelegate<SetMediaMetadata>().Invoke(mediaInstance, metadata, handle.AddrOfPinnedObject());
            handle.Free();
        }
    }
}

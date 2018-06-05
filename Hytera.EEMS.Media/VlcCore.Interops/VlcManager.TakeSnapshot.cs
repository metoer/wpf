using System;
using System.IO;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public void TakeSnapshot(VlcMediaPlayerInstance mediaPlayerInstance, FileInfo file, uint width, uint height)
        {
            if (mediaPlayerInstance == IntPtr.Zero)
                throw new ArgumentException("Media player instance is not initialized.");
            if(file == null)
                throw new ArgumentNullException("file");
            GetInteropDelegate<TakeSnapshot>().Invoke(mediaPlayerInstance, 0, System.Text.Encoding.UTF8.GetBytes(file.FullName), width, height);
        }
    }
}

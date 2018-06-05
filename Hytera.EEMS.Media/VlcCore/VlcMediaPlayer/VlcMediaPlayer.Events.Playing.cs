using Hytera.EEMS.Media.Signatures;
using System;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcMediaPlayer
    {
        private EventCallback myOnMediaPlayerPlayingInternalEventCallback;
        public event EventHandler<VlcMediaPlayerPlayingEventArgs> Playing;

        private void OnMediaPlayerPlayingInternal(IntPtr ptr)
        {
            OnMediaPlayerPlaying();
        }

        public void OnMediaPlayerPlaying()
        {
            var del = Playing;
            if (del != null)
                del(this, new VlcMediaPlayerPlayingEventArgs());
        }
    }
}
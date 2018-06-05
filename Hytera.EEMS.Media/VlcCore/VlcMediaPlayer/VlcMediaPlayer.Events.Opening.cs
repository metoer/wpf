using Hytera.EEMS.Media.Signatures;
using System;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcMediaPlayer
    {
        private EventCallback myOnMediaPlayerOpeningInternalEventCallback;
        public event EventHandler<VlcMediaPlayerOpeningEventArgs> Opening;

        private void OnMediaPlayerOpeningInternal(IntPtr ptr)
        {
            OnMediaPlayerOpening();
        }

        public void OnMediaPlayerOpening()
        {
            var del = Opening;
            if (del != null)
                del(this, new VlcMediaPlayerOpeningEventArgs());
        }
    }
}
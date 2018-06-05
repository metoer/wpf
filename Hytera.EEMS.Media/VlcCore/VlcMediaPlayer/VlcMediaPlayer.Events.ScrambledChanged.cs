﻿using Hytera.EEMS.Media.Signatures;
using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcMediaPlayer
    {
        private EventCallback myOnMediaPlayerScrambledChangedInternalEventCallback;
        public event EventHandler<VlcMediaPlayerScrambledChangedEventArgs> ScrambledChanged;

        private void OnMediaPlayerScrambledChangedInternal(IntPtr ptr)
        {
            var args = (VlcEventArg) Marshal.PtrToStructure(ptr, typeof (VlcEventArg));
            OnMediaPlayerScrambledChanged(args.MediaPlayerScrambledChanged.NewScrambled);
        }

        public void OnMediaPlayerScrambledChanged(int newScrambled)
        {
            var del = ScrambledChanged;
            if (del != null)
                del(this, new VlcMediaPlayerScrambledChangedEventArgs(newScrambled));
        }
    }
}
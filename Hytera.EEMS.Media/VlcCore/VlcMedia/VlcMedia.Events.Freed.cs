﻿using Hytera.EEMS.Media.Signatures;
using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media
{
    public partial class VlcMedia
    {
        private EventCallback myOnMediaFreedInternalEventCallback;
        public event EventHandler<VlcMediaFreedEventArgs> Freed;

        private void OnMediaFreedInternal(IntPtr ptr)
        {
            var args = (VlcEventArg) Marshal.PtrToStructure(ptr, typeof (VlcEventArg));
            //OnMediaFreed(args.MediaFreed.MediaInstance);
            OnMediaFreed();
        }

        public void OnMediaFreed()
        {
            var del = Freed;
            if (del != null)
                del(this, new VlcMediaFreedEventArgs());
        }
    }
}
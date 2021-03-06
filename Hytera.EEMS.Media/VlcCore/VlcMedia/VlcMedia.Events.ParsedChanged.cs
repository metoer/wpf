﻿using Hytera.EEMS.Media.Signatures;
using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media
{
    public partial class VlcMedia
    {
        private EventCallback myOnMediaParsedChangedInternalEventCallback;
        public event EventHandler<VlcMediaParsedChangedEventArgs> ParsedChanged;

        private void OnMediaParsedChangedInternal(IntPtr ptr)
        {
            var args = (VlcEventArg) Marshal.PtrToStructure(ptr, typeof (VlcEventArg));
            OnMediaParsedChanged(args.MediaParsedChanged.NewStatus);
        }

        public void OnMediaParsedChanged(int newStatus)
        {
            var del = ParsedChanged;
            if (del != null)
                del(this, new VlcMediaParsedChangedEventArgs(newStatus));
        }
    }
}
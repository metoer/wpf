﻿using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public void ParseMedia(VlcMediaInstance mediaInstance)
        {
            if (mediaInstance == IntPtr.Zero)
                throw new ArgumentException("Media instance is not initialized.");
            GetInteropDelegate<ParseMedia>().Invoke(mediaInstance);
        }
    }
}

﻿using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public void SetAudioDelay(VlcMediaPlayerInstance mediaPlayerInstance, long delay)
        {
            if (mediaPlayerInstance == IntPtr.Zero)
                throw new ArgumentException("Media player instance is not initialized.");
            GetInteropDelegate<SetAudioDelay>().Invoke(mediaPlayerInstance, delay);
        }
    }
}

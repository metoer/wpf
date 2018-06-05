﻿using System;
using System.Runtime.InteropServices;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public string GetMediaMrl(VlcMediaInstance mediaInstance)
        {
            if (mediaInstance == IntPtr.Zero)
                throw new ArgumentException("Media instance is not initialized.");
            var ptr = GetInteropDelegate<GetMediaMrl>().Invoke(mediaInstance);
            return Marshal.PtrToStringAnsi(ptr);
        }
    }
}

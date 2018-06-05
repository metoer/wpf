﻿using System;

namespace Hytera.EEMS.Media
{
    public sealed class VlcMediaPlayerSeekableChangedEventArgs : EventArgs
    {
        public VlcMediaPlayerSeekableChangedEventArgs(int newSeekable)
        {
            NewSeekable = newSeekable;
        }

        public int NewSeekable { get; private set; }
    }
}
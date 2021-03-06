﻿using System;

namespace Hytera.EEMS.Media
{
    public sealed class VlcMediaPlayerTitleChangedEventArgs : EventArgs
    {
        public VlcMediaPlayerTitleChangedEventArgs(string newTitle)
        {
            NewTitle = newTitle;
        }

        public string NewTitle { get; private set; }
    }
}
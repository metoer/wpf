﻿using System;
using System.Runtime.InteropServices;

namespace Hytera.EEMS.Media.Signatures
{
    /// <summary>
    /// Get an event's type name.
    /// </summary>
    [LibVlcFunction("libvlc_event_type_name")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr GetEventTypeName(EventTypes eventType);
}

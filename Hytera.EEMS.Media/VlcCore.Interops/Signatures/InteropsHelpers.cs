using System;
using System.Collections.Generic;
using System.Text;

namespace Hytera.EEMS.Media.Signatures
{
    internal static class InteropsHelpers
    {
#if X86
        public const int OFFSET_LENGTH_OF_POINTER = 4;
#else
        public const int OFFSET_LENGTH_OF_POINTER = 8;
#endif
    }
}

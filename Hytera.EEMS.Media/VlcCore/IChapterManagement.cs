using System;
using System.Collections.Generic;
using System.Text;

namespace Hytera.EEMS.Media
{
    public interface IChapterManagement
    {
        int Count { get; }
        void Previous();
        void Next();
        int Current { get; set; }
    }
}

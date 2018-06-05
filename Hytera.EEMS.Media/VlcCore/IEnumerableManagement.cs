using System.Collections.Generic;

namespace Hytera.EEMS.Media
{
    public interface IEnumerableManagement<T>
    {
        int Count { get; }
        T Current { get; set; }
        IEnumerable<T> All { get; }
    }
}

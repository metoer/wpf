using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public void ClearLastErrorMessage()
        {
            GetInteropDelegate<ClearLastErrorMessage>().Invoke();
        }
    }
}

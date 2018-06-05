using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public void CreateNewInstance(string[] args)
        {
            if (args == null)
                args = new string[0];
            myVlcInstance = new VlcInstance(this, GetInteropDelegate<CreateNewInstance>().Invoke(args.Length, args));
        }
    }
}

using System;
using Hytera.EEMS.Media.Signatures;

namespace Hytera.EEMS.Media
{
    public sealed partial class VlcManager
    {
        public void ReleaseTrackDescription(TrackDescriptionStructure trackDescription)
        {
            GetInteropDelegate<ReleaseTrackDescription>().Invoke(trackDescription);
        }
    }
}

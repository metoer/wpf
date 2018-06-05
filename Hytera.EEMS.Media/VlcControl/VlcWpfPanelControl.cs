using System.Windows.Forms.Integration;

namespace Hytera.EEMS.Media.Controls
{
    public class VlcWpfPanelControl : WindowsFormsHost
    {
        public VlcControl MediaPlayer 
        { 
            get;
            private set;
        }

        public VlcWpfPanelControl()
        {
            MediaPlayer = new VlcControl();
            this.Child = MediaPlayer;
        }
    }
}

using Hytera.EEMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hytera.EEMS.Main
{
    public static class EntryPoint
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if (SingleApplication.Run())
            {
                App app = new App();
                app.Run();   
            }
        }
    }
}

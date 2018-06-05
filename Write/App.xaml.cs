using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Write
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }
    }

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

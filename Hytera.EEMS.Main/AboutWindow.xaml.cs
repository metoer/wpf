using Hytera.EEMS.Dispatcher;
using Hytera.EEMS.Resources.Controls;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace Hytera.EEMS.Main
{
    /// <summary>
    /// AboutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : BaseWindow
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string day = TryFindResource("appMainDay").ToString();
            string fileExeName = Assembly.GetEntryAssembly().Location;
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(fileExeName);
            txtVersion.Text = string.Format(txtVersion.Text, info.FileVersion);

            txtEmpowerCountDays.Text = string.IsNullOrEmpty(AppConfigInfos.LicenseInfo.EmpowerCountDays) ?
                                       txtEmpowerCountDays.Text :
                                       (AppConfigInfos.LicenseInfo.EmpowerCountDays + day);

            txtEmpowerFileCreateTime.Text = string.IsNullOrEmpty(AppConfigInfos.LicenseInfo.EmpowerFileCreateTime) ?
                                       txtEmpowerFileCreateTime.Text :
                                       AppConfigInfos.LicenseInfo.EmpowerFileCreateTime;

            txtEmpowerFileEndTime.Text = string.IsNullOrEmpty(AppConfigInfos.LicenseInfo.EmpowerFileEndTime) ?
                                       txtEmpowerFileEndTime.Text :
                                       AppConfigInfos.LicenseInfo.EmpowerFileEndTime;

            txtEmpowerMachineCode.Text = string.IsNullOrEmpty(AppConfigInfos.LicenseInfo.EmpowerMachineCode) ?
                                       txtEmpowerMachineCode.Text :
                                       AppConfigInfos.LicenseInfo.EmpowerMachineCode;

            txtEmpowerSoftdog.Text = string.IsNullOrEmpty(AppConfigInfos.LicenseInfo.EmpowerSoftdog) ?
                                       txtEmpowerSoftdog.Text :
                                       AppConfigInfos.LicenseInfo.EmpowerSoftdog;

            txtEmpowerStatus.Text = string.IsNullOrEmpty(AppConfigInfos.LicenseInfo.EmpowerStatus) ?
                                        txtEmpowerStatus.Text :
                                        TryFindResource((AppConfigInfos.LicenseInfo.EmpowerStatus.Equals("1") ? "appMainValid" : "appMainInvalid")).ToString();

            txtEmpowerSurplusDays.Text = string.IsNullOrEmpty(AppConfigInfos.LicenseInfo.EmpowerSurplusDays) ?
                                       txtEmpowerSurplusDays.Text :
                                       (AppConfigInfos.LicenseInfo.EmpowerSurplusDays + day);

            txtIsMachineCode.Text = string.IsNullOrEmpty(AppConfigInfos.LicenseInfo.IsMachineCode) ?
                                       txtIsMachineCode.Text :
                                       TryFindResource((AppConfigInfos.LicenseInfo.IsMachineCode.Equals("1") ? "appYes" : "appNo")).ToString();

            txtIsSoftdogCode.Text = string.IsNullOrEmpty(AppConfigInfos.LicenseInfo.IsSoftdogCode) ?
                                       txtIsSoftdogCode.Text :
                                       TryFindResource((AppConfigInfos.LicenseInfo.IsSoftdogCode.Equals("1") ? "appYes" : "appNo")).ToString();

            txtLocalMachineCode.Content = string.IsNullOrEmpty(AppConfigInfos.LicenseInfo.LocalMachineCode) ?
                                       txtLocalMachineCode.Content :
                                       AppConfigInfos.LicenseInfo.LocalMachineCode;
            txtSoftdogCode.Text = string.IsNullOrEmpty(AppConfigInfos.LicenseInfo.SoftdogCode) ?
                                       txtSoftdogCode.Text :
                                       AppConfigInfos.LicenseInfo.SoftdogCode;
            
        }


        private void txtLocalMachineCode_Click(object sender, RoutedEventArgs e)
        {
            popupSet.IsOpen = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(txtLocalMachineCode.Content.ToString());
            popupSet.IsOpen = false;
        }
    }
}

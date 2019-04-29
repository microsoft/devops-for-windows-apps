using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using OSVersionHelper;
using Windows.ApplicationModel;
using Windows.Management.Deployment;
using WpfCoreApp.Telemetry;

namespace WpfCoreApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            versionText.Text = ThisAppInfo.GetDisplayName() + ThisAppInfo.GetThisAssemblyVersion();
            inPackage.Text = WindowsVersionHelper.HasPackageIdentity.ToString();
            deploymentType.Text = ThisAppInfo.GetDotNetInfo();
            packageVersion.Text = ThisAppInfo.GetPackageVersion();
            installedFrom.Text = ThisAppInfo.GetAppInstallerUri();

            DiagnosticsClient.TrackPageView(nameof(MainWindow));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonShowRuntimeVersionInfo.Content.ToString().StartsWith("Show"))
            {
                RuntimeVersionInfo.Text = ThisAppInfo.GetDotNetRuntimeInfo();
                DiagnosticsClient.TrackEvent("ClickShowRuntimeInfo");
                ButtonShowRuntimeVersionInfo.Content = "Hide Runtime Info";
            }
            else
            {
                RuntimeVersionInfo.Text = "";
                DiagnosticsClient.TrackEvent("ClickShowRuntimeInfo");
                ButtonShowRuntimeVersionInfo.Content = "Show Runtime Info";
            }
        }

        private async void ButtonCheckForUpdates_Click(object sender, RoutedEventArgs e)
        {
            DiagnosticsClient.TrackEvent("CheckForUpdates");
            if (WindowsVersionHelper.HasPackageIdentity)
            {
                var p = Package.Current;
                var updateInfo = await p.CheckUpdateAvailabilityAsync();
                UpdateInfo.Text = updateInfo.Availability.ToString();
                if (updateInfo.Availability==PackageUpdateAvailability.Available ||
                    updateInfo.Availability == PackageUpdateAvailability.Required)
                {
                    UpdateInfo.Text += " trying to apply update from: " + p.GetAppInstallerInfo().Uri.ToString();
                    var res = await CheckForUpdates(p);
                    UpdateInfo.Text = res;
                }
            }
            else
            {
                UpdateInfo.Text = "App not packaged, updates not available.";
            }
            
        }

        private static async Task<string> CheckForUpdates(Package p)
        {
            string result = string.Empty;
            try
            {
                var pm = new PackageManager();
                var res = await pm.UpdatePackageAsync(p.GetAppInstallerInfo().Uri, null, DeploymentOptions.ForceUpdateFromAnyVersion);
                result = res.ErrorText;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                DiagnosticsClient.TrackTrace(ex.ToString());
            }
            return result;
        }
    }
}

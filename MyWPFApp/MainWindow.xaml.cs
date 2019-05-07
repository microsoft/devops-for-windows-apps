using System.Reflection;
using System.Windows;
using OSVersionHelper;

using MyWPFApp.Telemetry;

namespace MyWPFApp
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
            DiagnosticsClient.TrackEvent("ClickShowRuntimeInfo");

            if (ButtonShowRuntimeVersionInfo.Content.ToString().StartsWith("Show"))
            {
                RuntimeVersionInfo.Text = ThisAppInfo.GetDotNetRuntimeInfo();                
                ButtonShowRuntimeVersionInfo.Content = "Hide Runtime Info";
            }
            else
            {
                RuntimeVersionInfo.Text = "";
                ButtonShowRuntimeVersionInfo.Content = "Show Runtime Info";
            }
        }
    }
}

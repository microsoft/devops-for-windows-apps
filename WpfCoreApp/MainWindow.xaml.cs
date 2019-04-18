using System.Reflection;
using System.Windows;
using OSVersionHelper;

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
            RuntimeVersionInfo.Text = ThisAppInfo.GetDotNetRuntimeInfo();
        }
    }
}

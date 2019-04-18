using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OSVersionHelper;
using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
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

            versionText.Text = GetDisplayName() +
                        typeof(MainWindow).Assembly
                            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                            .InformationalVersion ;

            inPackage.Text = WindowsVersionHelper.HasPackageIdentity.ToString();
            deploymentType.Text = GetDotNetInfo();
            packageVersion.Text = GetPackageVersion();
            installedFrom.Text = GetAppInstallerUri();
            DiagnosticsClient.TrackPageView(nameof(MainWindow));
        }

        private string GetDisplayName()
        {
            if (WindowsVersionHelper.HasPackageIdentity)
            {
                return Package.Current.DisplayName;
            }
            return "WpfCoreApp (dev)";
        }

        private string GetPackageVersion()
        {
            if (OSVersionHelper.WindowsVersionHelper.HasPackageIdentity)
            {
                return $"{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}.{Package.Current.Id.Version.Revision}";
            }
            return "Not Packaged";
        }

        public static string GetDotNetInfo()
        {
            var runTimeDir = new FileInfo(typeof(string).Assembly.Location);
            var entryDir = new FileInfo(Assembly.GetEntryAssembly().Location);
            var IsSelfContaied = runTimeDir.DirectoryName == entryDir.DirectoryName;

            var result = ".NET Core - ";
            if (IsSelfContaied)
            {
                result += "Self Contained Deployment";
            }
            else
            {
                result += "Framework Dependent Deployment";
            }
            
            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RuntimeVersionInfo.Text = typeof(object).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        }

        public string GetAppInstallerUri()
        {
            string result = string.Empty;

            if (OSVersionHelper.WindowsVersionHelper.HasPackageIdentity &&
                ApiInformation.IsMethodPresent("Windows.ApplicationModel.Package", "GetAppInstallerInfo"))
            {
                Uri aiUri = GetAppInstallerInfoUri(Package.Current);
                if (aiUri != null)
                {
                    result = aiUri.ToString();
                }
                else
                {
                    result = "not present";
                }
            }
            else
            {
                result = "not available";
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        Uri GetAppInstallerInfoUri(Package p)
        {
            var aiInfo = p.GetAppInstallerInfo();
            if (aiInfo != null)
            {
                return aiInfo.Uri;
            }
            return null;
        }
    }
}

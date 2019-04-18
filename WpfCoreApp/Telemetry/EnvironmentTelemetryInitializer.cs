using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace WpfCoreApp.Telemetry
{
    internal class EnvironmentTelemetryInitializer : ITelemetryInitializer
    {

#if CHANNEL_RELEASE
        private const string Channel = "Release";
#elif CHANNEL_CI
        private const string Channel = "CI";
#else
        private const string Channel = "Development";
#endif

        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.GlobalProperties["Environment"] = Channel;
            // Always default to Development if we're in the debugger
            if (Debugger.IsAttached)
            {
                telemetry.Context.GlobalProperties["Environment"] = "Development";
            }       
        }
    }
}

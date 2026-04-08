using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeCtrlClient.Helpers
{
    internal class LoggerHelper
    {
        public static ILogger GetDefaultLogger()
        {
            var logDir = "Logs";
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File($"{logDir}/EdgeCtrlClient.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}

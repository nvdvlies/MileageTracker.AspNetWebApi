using log4net;
using log4net.Config;
using MileageTracker.Infrastructure.Configuration;
using System;

namespace MileageTracker.Infrastructure.Logging {
    public class Log4NetAdapter : ILogger {
        private readonly ILog _log;

        public Log4NetAdapter() {
            XmlConfigurator.Configure();
            _log = LogManager.GetLogger(ApplicationSettingsFactory.GetApplicationSettings().LoggerName);
        }

        public void LogInfo(string message) {
            _log.Info(message);
        }

        public void LogError(string message, Exception ex) {
            _log.Error(message, ex);
        }
    }
}

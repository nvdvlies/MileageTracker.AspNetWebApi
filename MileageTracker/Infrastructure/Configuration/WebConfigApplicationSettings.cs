using System.Configuration;

namespace MileageTracker.Infrastructure.Configuration {
    public class WebConfigApplicationSettings : IApplicationSettings {
        public string LoggerName {
            get { return ConfigurationManager.AppSettings["LoggerName"]; }
        }
    }

}

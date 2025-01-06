
using System.Configuration;

namespace Utils.Helper
{
    public static class ConfigHelper
    {
        public static string GetSettings(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static void UpdateSettings(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = config.AppSettings.Settings;

            if (settings[key] != null)
            {
                settings[key].Value = value;
            }
            else
            {
                settings.Add(key, value);
            }
            config.Save();
        }
    }
}

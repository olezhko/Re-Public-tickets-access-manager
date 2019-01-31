using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace QrScanner.Model
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;
        private static readonly string SettingsName = "";
        private static readonly string SettingsPassword = "";
        private static readonly bool SettingsIsLogin = false;

        #endregion

        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }

        public static string Name
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(Name), SettingsName);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(Name), value);
            }
        }

        public static string Password
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(Password), SettingsPassword);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(Password), value);
            }
        }

        public static bool IsLogin
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(IsLogin), SettingsIsLogin);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(IsLogin), value);
            }
        }
    }
}

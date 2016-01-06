﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Hfr.Helpers
{
    public class ApplicationSettingsHelper
    {
        public ApplicationSettingsHelper()
        {
            ApplicationData.Current.DataChanged += Current_DataChanged;
        }
        
        private void Current_DataChanged(ApplicationData sender, object args)
        {
            Debug.WriteLine("App Data changed!");
        }

        /// <summary>
        /// Function that checks if the entry exists in Application settings
        /// </summary>
        public static bool Contains(string key, bool localSettings = true)
        {
            if (localSettings)
                return ApplicationData.Current.LocalSettings.Values.ContainsKey(key);
            else
                return ApplicationData.Current.RoamingSettings.Values.ContainsKey(key);
        }

        /// <summary>
        /// Function to read a setting value and clear it after reading it
        /// </summary>
        public static object ReadResetSettingsValue(string key, bool localSettings = true)
        {
            if (!Contains(key, localSettings))
            {
                return null;
            }
            else
            {
                var value = (localSettings) ? ApplicationData.Current.LocalSettings.Values[key] : ApplicationData.Current.RoamingSettings.Values[key];
                if (localSettings)
                    ApplicationData.Current.LocalSettings.Values.Remove(key);
                else
                    ApplicationData.Current.RoamingSettings.Values.Remove(key);
                return value;
            }
        }

        /// <summary>
        /// Function to read a setting value
        /// </summary>
        public static object ReadSettingsValue(string key, bool localSettings = true)
        {
            if (!Contains(key, localSettings))
            {
                return null;
            }
            else
            {
                var value = (localSettings) ? ApplicationData.Current.LocalSettings.Values[key] : ApplicationData.Current.RoamingSettings.Values[key];
                return value;
            }
        }

        /// <summary>
        /// Save a key value pair in settings. Create if it doesn't exist
        /// </summary>
        public static void SaveSettingsValue(string key, object value, bool localSettings = true)
        {
            if (!Contains(key, localSettings))
            {
                if (localSettings)
                    ApplicationData.Current.LocalSettings.Values.Add(key, value);
                else
                    ApplicationData.Current.RoamingSettings.Values.Add(key, value);
            }
            else
            {
                if (localSettings)
                    ApplicationData.Current.LocalSettings.Values[key] = value;
                else
                    ApplicationData.Current.RoamingSettings.Values[key] = value;
            }
        }
    }
}

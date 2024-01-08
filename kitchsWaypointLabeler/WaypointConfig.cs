using ProtoBuf;
using System;
using System.Collections.Generic;
using Vintagestory.API.Common;

namespace kitchsWaypointLabeler
{
    public static class WaypointConfig
    {
        public const string ConfigName = "kitchs-waypoint-labeler.json";
        public const string ModLabel = "[kitchs-waypoint-labeler]";

        // Cached set of settings
        public static WaypointSettings cachedClientSettings = null;

        public class WaypointSettings
        {
            // User values that are saved off
            public Dictionary<string, string> NameCache { get; set; }

            public bool DebugMode { get; set; }

            public WaypointSettings()
            {
                DebugMode = false;
                NameCache = new Dictionary<string, string>();
            }

            // Gets the saved value
            public string Get(string icon, string color)
            {
                string nameKey = string.Concat(icon, "-", color);
                if (NameCache.TryGetValue(nameKey, out string label))
                {
                    return label;
                }
                return "";
            }

            // Sets the saved value and returns true if something is updated.
            // To prevent the uneccesary saving if nothing changes.
            public bool Set(string icon, string color, string label)
            {
                string nameKey = string.Concat(icon, "-", color);

                if (NameCache.ContainsKey(nameKey))
                {
                    if (NameCache[nameKey] != label)
                    {
                        NameCache[nameKey] = label;
                        return true;
                    }
                }
                else
                {
                    NameCache.Add(nameKey, label);
                    return true;
                }
                return false;
            }
        }

        public static WaypointSettings GetSettings(ICoreAPI api)
        {
            if (api.Side != EnumAppSide.Client)
            {
                return null;
            }
            // If we already loaded it, just return it.
            if (cachedClientSettings != null) { return cachedClientSettings; }


            WaypointSettings settings = null;
            try
            {
                settings = api.LoadModConfig<WaypointSettings>(ConfigName);
                cachedClientSettings = settings;
            }
            catch
            {
                // Theory is this throws an exception when the file isn't there, we'll see...
                settings = null;
                api.Logger.Log(EnumLogType.Notification, string.Concat(ModLabel, " Unable to load config file '", ConfigName, "'.  Creating you a fresh new one."));
            }

            // If it wasn't loaded, create a new one and save it off.
            if (settings == null)
            {
                settings = new WaypointSettings();
                SaveSettings(api, settings);
            }
            return settings;
        }

        public static void SaveSettings(ICoreAPI api, WaypointSettings settings)
        {
            if (api.Side != EnumAppSide.Client)
            {
                return;
            }
            cachedClientSettings = settings;
            api.StoreModConfig<WaypointSettings>(settings, ConfigName);
        }
    }
}

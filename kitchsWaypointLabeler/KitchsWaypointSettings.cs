using ProtoBuf;
using System;
using System.Collections.Generic;
using Vintagestory.API.Common;

namespace kitchsWaypointLabeler
{
    public static class KitchsWaypointSettings
    {
        public const string ConfigName = "kitchs-waypoint-labels-config.json";
        public const string ModLabel = "[kitchs-waypoint-labels]";

        public static WaypointSettings cachedClientSettings = null;

        [ProtoContract]
        public class WaypointSettings
        {
            [ProtoMember(1)]
            public Dictionary<string, string> NameCache { get; set; }


            public WaypointSettings()
            {
                NameCache = new Dictionary<string, string>();
            }

            public string Get(string icon, string color)
            {
                string nameKey = string.Concat(icon, "-", color);
                if (NameCache.TryGetValue(nameKey, out string label))
                {
                    return label;
                }
                return "";
            }

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
            if (api.Side == EnumAppSide.Server || api.Side == EnumAppSide.Universal)
            {
                return null;
            }

            if (cachedClientSettings != null) { return cachedClientSettings; }
            WaypointSettings settings = null;

            try
            {
                settings = api.LoadModConfig<WaypointSettings>(ConfigName);
                if (api.Side == EnumAppSide.Client)
                    cachedClientSettings = settings;
            }
            catch
            {
                settings = null;
                api.Logger.Log(EnumLogType.Warning, string.Concat(ModLabel, " Unable to load config file '", ConfigName, "'.  Creating you a fresh new one."));
            }

            if (settings == null)
            {
                settings = new WaypointSettings();
                SaveSettings(api, settings);
            }
            return settings;
        }

        public static void SaveSettings(ICoreAPI api, WaypointSettings settings)
        {
            if (api.Side == EnumAppSide.Server || api.Side == EnumAppSide.Universal)
            {
                return;
            }
            cachedClientSettings = settings;
            api.StoreModConfig<WaypointSettings>(settings, ConfigName);
        }
    }
}

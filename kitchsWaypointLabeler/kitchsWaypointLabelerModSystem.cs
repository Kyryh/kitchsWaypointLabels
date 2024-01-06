using HarmonyLib;
using System.Formats.Asn1;
using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using static kitchsWaypointLabeler.KitchsWaypointSettings;

[assembly: ModInfo("Kitch's Waypoint Label Rememberer", "kitchswaypointlabels",
                    Authors = new string[] { "kitch" },
                    Description = "A mod that remembers previously used waypoint labels.",
                    Version = "0.1.0")]

namespace kitchsWaypointLabeler
{
    public class kitchsWaypointLabelerModSystem : ModSystem
    {
        public static ICoreAPI CoreApi;
        public Harmony harmony;

        public override void Start(ICoreAPI api)
        {
            api.Logger.Notification("Loading Kitch's Waypoints Rememberer"); 
            CoreApi = api;
            harmony = new Harmony("kitchswaypointlabels");
            var original = typeof(Vintagestory.GameContent.GuiDialogAddWayPoint).GetMethod("autoSuggestName", BindingFlags.Instance | BindingFlags.NonPublic);
            var replacer = typeof(KitchsPatches).GetMethod("Prefix_autoSuggestName");
            
            if( original == null ) 
            {
                api.Logger.Log(EnumLogType.Error, "Couldn't Find GuiDialogAddWayPoint.autoSuggestName");
                return;
            }

            harmony.Patch(original, new HarmonyMethod(replacer));

            original = typeof(Vintagestory.GameContent.GuiDialogAddWayPoint).GetMethod("onSave", BindingFlags.Instance | BindingFlags.NonPublic);
            replacer = typeof(KitchsPatches).GetMethod("Postfix_onSave");

            if (original == null)
            {
                api.Logger.Log(EnumLogType.Error, "Couldn't Find GuiDialogAddWayPoint.onSave");
                return;
            }
            harmony.Patch(original, null, new HarmonyMethod(replacer));
            api.Logger.Notification("Loaded Kitch's Waypoints");
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
        }
    }
}

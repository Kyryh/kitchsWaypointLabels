using HarmonyLib;
using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

[assembly: ModInfo("Kitch's Waypoint Labeler", "kitchwaypointlabeler",
                    Authors = new string[] { "kitch" },
                    Description = "A mod that remembers previously used waypoint labels.",
                    Version = "1.1.0")]

namespace kitchsWaypointLabeler
{
    public class WaypointLabelerModSystem : ModSystem
    {
        public const string harmonyId = "kitchwaypointlabeler";
        public static ICoreAPI CoreApi;
        public Harmony harmony;

        public override void Start(ICoreAPI api)
        {
        }
        public override void StartServerSide(ICoreServerAPI api)
        {
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            api.Logger.Notification(string.Concat(WaypointConfig.ModLabel, " Loading Kitch's Waypoint Labeler"));
            CoreApi = api;
            harmony = new Harmony(harmonyId);

            // Pulling the methods via reflection first, just to make sure they are there.
            // The idea here is to prevent this mod from crashing everything if the functions change or are removed.  I have no idea if this works.... we'll see. 
            // These are both "private" methods, so the 'BindingFlags.Instance | BindingFlags.NonPublic' flags are needed or the method will not be found.
            var originalAutoSuggestName = typeof(Vintagestory.GameContent.GuiDialogAddWayPoint).GetMethod("autoSuggestName", BindingFlags.Instance | BindingFlags.NonPublic);
            var originalOnSave = typeof(Vintagestory.GameContent.GuiDialogAddWayPoint).GetMethod("onSave", BindingFlags.Instance | BindingFlags.NonPublic);

            if ((originalAutoSuggestName != null) && (originalOnSave != null))
            {
                var replacerAutoSuggestName = typeof(WaypointPatches).GetMethod("Prefix_autoSuggestName");
                var replacerOnSave = typeof(WaypointPatches).GetMethod("Postfix_onSave");

                // We're removing the autosuggest all together, so prefix it to overwrite it
                harmony.Patch(originalAutoSuggestName, new HarmonyMethod(replacerAutoSuggestName));

                // On save is just an append to the end of the save method.
                harmony.Patch(originalOnSave, null, new HarmonyMethod(replacerOnSave));

                api.Logger.Notification(string.Concat(WaypointConfig.ModLabel, " Loaded Kitch's Waypoint Labeler"));
            }
            else
            {
                // Something wasn't found, so spit out why... and don't load the mod.
                if (originalAutoSuggestName == null)
                {
                    api.Logger.Log(EnumLogType.Error, string.Concat(WaypointConfig.ModLabel, " Couldn't Find GuiDialogAddWayPoint.autoSuggestName"));
                }
                if (originalOnSave == null)
                {
                    api.Logger.Log(EnumLogType.Error, string.Concat(WaypointConfig.ModLabel, " Couldn't Find GuiDialogAddWayPoint.onSave"));
                }
            }           
        }

        public override void Dispose()
        {
            harmony.UnpatchAll(harmonyId);
            base.Dispose();
        }
    }
}

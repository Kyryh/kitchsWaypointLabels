using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace kitchsWaypointLabeler
{
    public class WaypointPatches
    {

        // __instace grabs the instance the method is called from
        // "curIcon" and "curColor" (the user selections) are private variables in the GuiDialogAddWayPoint class so inaccessible through __instance.
        // Harmony magic pulls those out of the class and makes them accessible when you pass them by ref and add the "___" to it.
        public static bool Prefix_autoSuggestName(Vintagestory.GameContent.GuiDialogAddWayPoint __instance, ref string ___curIcon, ref string ___curColor)
        {
            WaypointConfig.WaypointSettings settings = WaypointConfig.GetSettings(WaypointLabelerModSystem.CoreApi);
            // if for some reason, the settings aren't loaded, just run the old autonamer thing and let it be Copper or Ore or Rocks or whatever.            
            if ( settings == null) { return true; }

            if (settings.DebugMode)
            {
                WaypointLabelerModSystem.CoreApi.Logger.Log(EnumLogType.Notification, string.Concat(WaypointConfig.ModLabel, " autoSuggestName - Icon='", ___curIcon, "' Color='", ___curColor, "'"));
            }

            string name = settings.Get(___curIcon, ___curColor);

            // if there isn't anything defined, just run the old autonamer thing and let it be Copper or Ore or Rocks or whatever.           
            if (string.IsNullOrEmpty(name))
            {
                if (settings.DebugMode)
                {
                    WaypointLabelerModSystem.CoreApi.Logger.Log(EnumLogType.Notification, string.Concat(WaypointConfig.ModLabel, " autoSuggestName - Value was not found"));
                }
                return true;
            }
            else if (settings.DebugMode)
            {
                WaypointLabelerModSystem.CoreApi.Logger.Log(EnumLogType.Notification, string.Concat(WaypointConfig.ModLabel, " autoSuggestName - Value='", name, "' was found"));
            }

            // This is the text box we want to populate with the new value.
            // Couldn't figure out a way to access the "base" class, so I did it this way and it seemed to work.
            GuiDialog dlg = (GuiDialog)__instance;
            GuiElementTextInput textElem = dlg.SingleComposer.GetTextInput("nameInput");
            textElem.SetValue(name, true);

            // Returning false here causes the rest of the "autoSuggestName" code to not run.
            return false;
        }

        // __instace grabs the instance the method is called from
        // "curIcon" and "curColor" (the user selections) are private variables in the GuiDialogAddWayPoint class so inaccessible through __instance.
        // Harmony magic pulls those out of the class and makes them accessible when you pass them by ref and add the "___" to it.
        public static void Postfix_onSave(Vintagestory.GameContent.GuiDialogAddWayPoint __instance, ref string ___curIcon, ref string ___curColor)
        {
            WaypointConfig.WaypointSettings settings = WaypointConfig.GetSettings(WaypointLabelerModSystem.CoreApi);

            if (settings == null) { return; }

            // This is the text box that has the value we want to remember.
            // Couldn't figure out a way to access the "base" class, so I did it this way and it seemed to work.
            GuiDialog dlg = (GuiDialog)__instance;
            string name = dlg.SingleComposer.GetTextInput("nameInput").GetText();

            if (settings.DebugMode)
            {
                WaypointLabelerModSystem.CoreApi.Logger.Log(EnumLogType.Notification, string.Concat(WaypointConfig.ModLabel, " onSave - Icon='", ___curIcon, "' Color='", ___curColor, "', Value='", name, "'"));
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                if (settings.Set(___curIcon, ___curColor, name)) // will return true, only if something updated.
                {
                    WaypointConfig.SaveSettings(WaypointLabelerModSystem.CoreApi, settings);

                    if (settings.DebugMode)
                    {
                        WaypointLabelerModSystem.CoreApi.Logger.Log(EnumLogType.Notification, string.Concat(WaypointConfig.ModLabel, " onSave - Value='", name, "' was updated"));
                    }

                }
                else if(settings.DebugMode)
                {
                    WaypointLabelerModSystem.CoreApi.Logger.Log(EnumLogType.Notification, string.Concat(WaypointConfig.ModLabel, " onSave - Value='", name, "' was already there"));
                }
            }
            else if( settings.DebugMode )
            {
                WaypointLabelerModSystem.CoreApi.Logger.Log(EnumLogType.Notification, string.Concat(WaypointConfig.ModLabel, " onSave - Value='", name, "' looks to be blank"));
            }
        }
    }
}

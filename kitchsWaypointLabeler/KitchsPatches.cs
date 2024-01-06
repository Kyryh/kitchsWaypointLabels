using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static kitchsWaypointLabeler.KitchsWaypointSettings;
using Vintagestory.API.Client;

namespace kitchsWaypointLabeler
{
    public class KitchsPatches
    {
        public static bool Prefix_autoSuggestName(Vintagestory.GameContent.GuiDialogAddWayPoint __instance, ref string ___curIcon, ref string ___curColor)
        {            
            WaypointSettings settings = KitchsWaypointSettings.GetSettings(kitchsWaypointLabelerModSystem.CoreApi);

            if( settings == null) { return true; }  // let it be Copper, or ore, or whatever

            string name = settings.Get(___curIcon, ___curColor);

            if( string.IsNullOrEmpty(name) ) { return true; } // let it be Copper, or ore, or whatever

            //kitchsWaypointLabelerModSystem.CoreApi.Logger.Log(Vintagestory.API.Common.EnumLogType.Notification, string.Concat("kitch-autoSuggest-", name, "-", ___curIcon, "-", ___curColor));
            GuiDialog dlg = (GuiDialog)__instance;
            GuiElementTextInput textElem = dlg.SingleComposer.GetTextInput("nameInput");
            textElem.SetValue(name, true);
            return false;
        }

        public static void Postfix_onSave(Vintagestory.GameContent.GuiDialogAddWayPoint __instance, ref string ___curIcon, ref string ___curColor)
        {
            GuiDialog dlg = (GuiDialog)__instance;
            string name = dlg.SingleComposer.GetTextInput("nameInput").GetText();
            kitchsWaypointLabelerModSystem.CoreApi.Logger.Log(Vintagestory.API.Common.EnumLogType.Notification, string.Concat("kitch-onSave-", name, "-", ___curIcon, "-", ___curColor));
            if (!string.IsNullOrWhiteSpace(name))
            {
                WaypointSettings settings = KitchsWaypointSettings.GetSettings(kitchsWaypointLabelerModSystem.CoreApi);

                if (settings != null && settings.Set(___curIcon, ___curColor, name)) // will return true, only if something updated.
                {
                    KitchsWaypointSettings.SaveSettings(kitchsWaypointLabelerModSystem.CoreApi, settings);
                }
            }
        }
    }
}

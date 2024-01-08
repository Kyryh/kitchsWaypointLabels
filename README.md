# Kitch's Waypoint Labels
A mod that remembers previously used waypoint labels and auto populates them when you select an icon and a color.

### What vanilla does
When you add a waypoint to the map and select an icon and a color, sometimes the UI will auto-populate the name.  Sometimes it will auto fill the "name" with a helpful description, so you do not need to type in a label. (Like when you select the "pick" icon and the color "orange", it will auto-fill "Copper") More times, it will not fill in anything, or provide a generic and generally unhelpful label like "Ore" or "Rocks".  

### What this mod does
When you add your first waypoint to the map, as usual you will select a color, icon and type in a name.  When you click "Save", the name you type will be saved along with the color/icon combination.  
Next time you add a waypoint and select the same icon/color combination, your last used name should be auto-populated.
Over time, your combinations and labels will be saved with the waypoint system you have going on in your head, and your typing needs should drop considerably.

### Change Log

#### 1.0.0

* Initial Release for Vintage Story 1.18 and 1.19
* Added a whole bunch of bugs that I don't know about, but will probably be fixed in a future version.

### Compatibility Notes

This mod tweaks a few calls in the *Vintagestory.GameContent.GuiDialogAddWayPoint* class.  
* Prefix on the *autoSuggestName* call to override it's behaviour
* Postfix ont he *onSave* call to append the saved label to the call
Any mod that also tweaks those calls may end up being a problem.

Should be compatible with mods that add additional map icons/colors.

It may fight with mods that automatically add map markers, or allow for more elaborate map marker naming schemes.  But I'm not sure why you would want to install both...







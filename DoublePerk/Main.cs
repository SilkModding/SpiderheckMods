using Silk;
using Logger = Silk.Logger;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace DoublePerk
{
    [SilkMod("Double Perk", new[] { "Abstractmelon" }, "1.0.0", "0.6.1", "double-perk", 1)]
    public class DoublePerkMod : SilkMod
    {
        public const string ModId = "double-perk";

        private Harmony _harmony;
        public static bool secondPickActive = false;
        public static Modifier firstPickedModifier = null;
        public static GameLevel firstPickedLevel = null;

        public override void Initialize()
        {
            Logger.LogInfo("Initializing Double Perk Mod...");

            var defaultConfig = new Dictionary<string, object>
            {
                { "enabled", true }
            };
            Config.LoadModConfig(ModId, defaultConfig);

            _harmony = new Harmony("com.modder.doubleperk");
            _harmony.PatchAll(typeof(Patches));

            Logger.LogInfo("Harmony patches applied.");
        }

        public void Awake()
        {
            Logger.LogInfo("Awake called.");
        }

        public override void Unload()
        {
            Logger.LogInfo("Unloading Double Perk Mod...");
            _harmony.UnpatchSelf();
        }
    }

    public static class Patches
    {
        // Patch the method that handles perk selection in Survival mode
        [HarmonyPatch(typeof(SurvivalModeHud), "PerkChoiceSubmit")]
        [HarmonyPrefix]
        public static bool PerkChoiceSubmitPatch(SurvivalModeHud __instance, Modifier modifier, GameLevel level, int index)
        {
            if (!Config.GetModConfigValue(DoublePerkMod.ModId, "enabled", true))
                return true;

            // If this is the first pick, store it and allow another pick
            if (!DoublePerkMod.secondPickActive)
            {
                DoublePerkMod.firstPickedModifier = modifier;
                DoublePerkMod.firstPickedLevel = level;
                DoublePerkMod.secondPickActive = true;
                
                Logger.LogInfo($"========================================");
                Logger.LogInfo($"FIRST PERK SELECTED: {modifier.data.title}");
                Logger.LogInfo($"PICK ANOTHER PERK NOW!");
                Logger.LogInfo($"========================================");
                
                // Reset the cards to allow another selection
                // This prevents the original method from closing the selection screen
                return false;
            }
            else
            {
                // This is the second pick, apply both perks
                Logger.LogInfo($"Second perk selected: {modifier.data.title}. Applying both perks!");
                
                DoublePerkMod.secondPickActive = false;
                
                // Let the original method handle the second perk
                // We'll apply the first one manually in a postfix
                return true;
            }
        }

        [HarmonyPatch(typeof(SurvivalModeHud), "PerkChoiceSubmit")]
        [HarmonyPostfix]
        public static void PerkChoiceSubmitPostfix(SurvivalModeHud __instance)
        {
            if (!Config.GetModConfigValue(DoublePerkMod.ModId, "enabled", true))
                return;

            // If we have a stored first pick, apply it now
            if (DoublePerkMod.firstPickedModifier != null)
            {
                Logger.LogInfo($"Applying first perk: {DoublePerkMod.firstPickedModifier.data.title}");
                
                // Apply the first modifier by incrementing its level
                DoublePerkMod.firstPickedModifier.levelInSurvival++;
                
                // Clear the stored values
                DoublePerkMod.firstPickedModifier = null;
                DoublePerkMod.firstPickedLevel = null;
            }
        }
    }
}

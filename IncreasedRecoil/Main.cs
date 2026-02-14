using Silk;
using Logger = Silk.Logger;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace IncreasedRecoil
{
    [SilkMod("Increased Recoil", new[] { "Abstractmelon" }, "1.0.0", "0.7.0", "increased-recoil", 1)]
    public class IncreasedRecoilMod : SilkMod
    {
        public const string ModId = "increased-recoil";

        private Harmony _harmony;

        public override void Initialize()
        {
            Logger.LogInfo("Initializing Increased Recoil Mod...");

            var defaultConfig = new Dictionary<string, object>
            {
                { "enabled", true },
                { "recoilMultiplier", 3.0f }
            };
            Config.LoadModConfig(ModId, defaultConfig);

            _harmony = new Harmony("com.modder.increasedrecoil");
            _harmony.PatchAll(typeof(Patches));

            Logger.LogInfo("Harmony patches applied.");
        }

        public void Awake()
        {
            Logger.LogInfo("Awake called.");
        }

        public override void Unload()
        {
            Logger.LogInfo("Unloading Increased Recoil Mod...");
            _harmony.UnpatchSelf();
        }
    }

    public static class Patches
    {
        // Patch Weapon's Impact method to multiply recoil when weapons hit things
        [HarmonyPatch(typeof(Weapon), "Impact")]
        [HarmonyPrefix]
        public static void ImpactPrefix(Weapon __instance, ref Vector3 force)
        {
            if (!Config.GetModConfigValue(IncreasedRecoilMod.ModId, "enabled", true))
                return;

            if (__instance.equipped)
            {
                // Get recoil multiplier from config
                float recoilMultiplier = Config.GetModConfigValue(IncreasedRecoilMod.ModId, "recoilMultiplier", 3.0f);
                
                // Multiply the recoil force
                force *= recoilMultiplier;
            }
        }

        // Patch Weapon's Disarm method to multiply recoil
        [HarmonyPatch(typeof(Weapon), "Disarm")]
        [HarmonyPrefix]
        public static void DisarmPrefix(Weapon __instance, ref Vector3 force)
        {
            if (!Config.GetModConfigValue(IncreasedRecoilMod.ModId, "enabled", true))
                return;

            if (__instance.equipped)
            {
                // Get recoil multiplier from config
                float recoilMultiplier = Config.GetModConfigValue(IncreasedRecoilMod.ModId, "recoilMultiplier", 3.0f);
                
                // Multiply the disarm force
                force *= recoilMultiplier;
            }
        }
    }
}

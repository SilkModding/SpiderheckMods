using Silk;
using Logger = Silk.Logger;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace NoSwordShrink
{
    [SilkMod("No Sword Shrink", new[] { "Abstractmelon" }, "1.0.0", "0.7.0", "no-sword-shrink", 1)]
    public class NoSwordShrinkMod : SilkMod
    {
        public const string ModId = "no-sword-shrink";

        private Harmony _harmony;

        public override void Initialize()
        {
            Logger.LogInfo("Initializing No Sword Shrink Mod...");

            var defaultConfig = new Dictionary<string, object>
            {
                { "enabled", true }
            };
            Config.LoadModConfig(ModId, defaultConfig);

            _harmony = new Harmony("com.abstractmelon.noswordshrink");
            _harmony.PatchAll(typeof(Patches));

            Logger.LogInfo("Harmony patches applied.");
        }

        public void Awake()
        {
            Logger.LogInfo("Awake called.");
        }

        public override void Unload()
        {
            Logger.LogInfo("Unloading No Sword Shrink Mod...");
            _harmony.UnpatchSelf();
        }
    }

    public static class Patches
    {
        // Patch GetCurrentMaxBladeSize to prevent shrinking based on ammo
        [HarmonyPatch(typeof(ParticleBlade), "GetCurrentMaxBladeSize")]
        [HarmonyPostfix]
        public static void GetCurrentMaxBladeSizePostfix(ParticleBlade __instance, ref Vector2 __result)
        {
            if (!Config.GetModConfigValue(NoSwordShrinkMod.ModId, "enabled", true))
                return;

            // Return the base size without the ammo-based shrinking
            // Original formula: baseSize * num * (ammo + 1) / maxAmmo
            // We want: baseSize * num (without the ammo scaling)
            float chargeScale = Mathf.Clamp(1f - __instance._charge / (__instance.maxChargeTime * __instance.chargeRatePerSec), 0.1f, 1f);
            __result = new Vector2(__instance.baseSize.x * chargeScale, __instance.baseSize.y * chargeScale);
        }
    }
}

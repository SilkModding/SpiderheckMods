using Silk;
using Logger = Silk.Logger;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace BiggerBombs
{
    [SilkMod("Bigger Bombs", new[] { "Abstractmelon" }, "1.0.0", "0.7.0", "bigger-bombs", 1)]
    public class BiggerBombsMod : SilkMod
    {
        public const string ModId = "bigger-bombs";

        private Harmony _harmony;

        public override void Initialize()
        {
            Logger.LogInfo("Initializing Bigger Bombs Mod...");

            var defaultConfig = new Dictionary<string, object>
            {
                { "enabled", true },
                { "explosionSizeMultiplier", 2.0f }
            };
            Config.LoadModConfig(ModId, defaultConfig);

            _harmony = new Harmony("com.abstractmelon.biggerbombs");
            _harmony.PatchAll(typeof(Patches));

            Logger.LogInfo("Harmony patches applied.");
        }

        public void Awake()
        {
            Logger.LogInfo("Awake called.");
        }

        public override void Unload()
        {
            Logger.LogInfo("Unloading Bigger Bombs Mod...");
            _harmony.UnpatchSelf();
        }
    }

    public static class Patches
    {
        // Patch Explosion's Start method to increase explosion size
        [HarmonyPatch(typeof(Explosion), "Start")]
        [HarmonyPostfix]
        public static void ExplosionStartPostfix(Explosion __instance)
        {
            if (!Config.GetModConfigValue(BiggerBombsMod.ModId, "enabled", true))
                return;

            float sizeMultiplier = Config.GetModConfigValue(BiggerBombsMod.ModId, "explosionSizeMultiplier", 2.0f);

            // Scale up the explosion radius and visual size
            __instance.deathRadius *= sizeMultiplier;
            __instance.knockBackRadius *= sizeMultiplier;
            __instance.transform.localScale *= sizeMultiplier;

            // Also scale up all child objects (particles, lights, etc)
            for (int i = 0; i < __instance.transform.childCount; i++)
            {
                Transform child = __instance.transform.GetChild(i);
                child.localScale *= sizeMultiplier;
            }

            Logger.LogInfo($"Explosion scaled to {sizeMultiplier}x size!");
        }
    }
}

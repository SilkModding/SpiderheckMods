using Silk;
using Logger = Silk.Logger;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace NoLava
{
    [SilkMod("No Lava", new[] { "Abstractmelon" }, "1.0.0", "0.7.0", "no-lava", 1)]
    public class NoLava : SilkMod
    {
        public const string ModId = "no-lava";

        private Harmony _harmony;

        public override void Initialize()
        {
            Logger.LogInfo("Initializing NoLava Mod...");

            var defaultConfig = new Dictionary<string, object>
            {
                { "DisableLava", true }
            };
            Config.LoadModConfig(ModId, defaultConfig);

            _harmony = new Harmony("com.abstractmelon.no-lava");
            _harmony.PatchAll(typeof(Patches));

            Logger.LogInfo("Harmony patches applied.");
        }

        public void Awake()
        {
            Logger.LogInfo("Awake called.");
        }

        public override void Unload()
        {
            Logger.LogInfo("Unloading Silk Mod...");
            _harmony.UnpatchSelf();
        }
    }

    public static class Patches
    {
        [HarmonyPatch(typeof(DeathZone), "Start")]
        [HarmonyPrefix]
        public static bool DisableLava()
        {
            GameObject.FindObjectOfType<DeathZone>().gameObject.SetActive(false);
            return false;
        }
    }
}

using Silk;
using Logger = Silk.Logger;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System;
using NoLavaSilk;

namespace NoLavaSilk
{
    [SilkMod("Silk Mod", new[] { "niiK0" }, "1.0.2", "0.6.1", "silk-mod", 1)]
    public class NoLavaMod : SilkMod
    {
        public const string ModId = "silk-mod";

        private Harmony _harmony;

        public override void Initialize()
        {
            Logger.LogInfo("Initializing NoLava Mod...");

            var defaultConfig = new Dictionary<string, object>
            {
                { "DisableLava", true }
            };
            Config.LoadModConfig(ModId, defaultConfig);

            _harmony = new Harmony("com.niiko.nolava");
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

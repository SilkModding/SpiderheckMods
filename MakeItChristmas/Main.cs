using Silk;
using Logger = Silk.Logger;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace MakeItChristmas
{
    [SilkMod("Make It Christmas", new[] { "Abstractmelon" }, "1.0.0", "0.6.1", "make-it-christmas", 1)]
    public class MakeItChristmas : SilkMod
    {
        public const string ModId = "make-it-christmas";

        private Harmony _harmony;

        public override void Initialize()
        {
            Logger.LogInfo("Initializing Make It Christmas...");

            // Load config with default "enableChristmas" set to true
            var defaultConfig = new Dictionary<string, object>
            {
                { "enableChristmas", true }
            };
            Config.LoadModConfig(ModId, defaultConfig);

            _harmony = new Harmony("com.Abstractmelon.MakeItChristmas");
            _harmony.PatchAll(typeof(Patches));

            Logger.LogInfo("Harmony patches applied.");
        }

        public void Awake()
        {
            Logger.LogInfo("Awake called.");
        }

        public override void Unload()
        {
            Logger.LogInfo("Unloading Make It Christmas...");
            _harmony.UnpatchSelf();
        }
    }

    public static class Patches
    {
        [HarmonyPatch(typeof(SeasonChecker), nameof(SeasonChecker.IsItChristmas))]
        [HarmonyPrefix]
        public static bool MakeItChristmasPatch(ref bool __result)
        {
            __result = Config.GetModConfigValue(MakeItChristmas.ModId, "enableChristmas", true);
            return false;
        }
    }
}


using Silk;
using Logger = Silk.Logger;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace InfiniteAmmo
{
    [SilkMod("Infinite Ammo", new[] { "Abstractmelon" }, "1.0.0", "0.7.0", "infinite-ammo", 1)]
    public class InfiniteAmmoMod : SilkMod
    {
        public const string ModId = "infinite-ammo";

        private Harmony _harmony;

        public override void Initialize()
        {
            Logger.LogInfo("Initializing Infinite Ammo Mod...");

            var defaultConfig = new Dictionary<string, object>
            {
                { "enabled", true }
            };
            Config.LoadModConfig(ModId, defaultConfig);

            _harmony = new Harmony("com.abstractmelon.infiniteammo");
            _harmony.PatchAll(typeof(Patches));

            Logger.LogInfo("Harmony patches applied.");
        }

        public void Awake()
        {
            Logger.LogInfo("Awake called.");
        }

        public override void Unload()
        {
            Logger.LogInfo("Unloading Infinite Ammo Mod...");
            _harmony.UnpatchSelf();
        }
    }

    public static class Patches
    {
        [HarmonyPatch(typeof(Weapon), nameof(Weapon.ammo), MethodType.Getter)]
        public class AmmoGetterPatch
        {
            public static bool Prefix(Weapon __instance, ref float __result)
            {
                __result = __instance.maxAmmo;
                return false;
            }
        }
    }
}

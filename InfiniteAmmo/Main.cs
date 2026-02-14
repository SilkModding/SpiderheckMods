using Silk;
using Logger = Silk.Logger;
using HarmonyLib;
using UnityEngine;

namespace InfiniteAmmo
{
    [SilkMod("Infinite Ammo", new[] { "Abstractmelon" }, "1.0.0", "0.7.0", "infinite-ammo", 1)]
    public class InfiniteAmmoMod : SilkMod
    {
        private Harmony _harmony;

        public override void Initialize()
        {
            Logger.LogInfo("Initializing Infinite Ammo Mod...");
            
            _harmony = new Harmony("com.abstractmelon.infiniteammo");
            _harmony.Patch(
                typeof(Weapon).GetMethod("FixedUpdate"),
                postfix: new HarmonyMethod(typeof(Patches), nameof(Patches.FixedUpdatePostfix))
            );

            Logger.LogInfo("Infinite Ammo Mod initialized successfully.");
        }

        public override void Unload()
        {
            _harmony?.UnpatchSelf();
        }
    }

    [HarmonyPatch]
    public static class Patches
    {
        public static void FixedUpdatePostfix(Weapon __instance)
        {
            // Only run on server/host (clients can't modify NetworkVariables)
            if (__instance.NetworkManager != null && 
                (!__instance.NetworkManager.IsClient || __instance.NetworkManager.IsHost))
            {
                // Restore ammo to max if below threshold
                if (__instance.ammo < __instance.maxAmmo - 0.01f)
                {
                    __instance.ammo = __instance.maxAmmo;
                }
            }
        }
    }
}
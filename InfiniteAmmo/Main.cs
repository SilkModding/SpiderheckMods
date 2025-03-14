using System.Collections;
using Silk;
using Logger = Silk.Logger; // Alias for Silk.Logger to Logger
using HarmonyLib; // Library for runtime method patching

namespace InfiniteAmmo
{
    [SilkMod("Infinite Ammo", new[] { "Abstractmelon" }, "1.0.0", "0.5.0", "infinite-ammo", 1)]
    public class InfiniteAmmo : SilkMod
    {
        public override void Initialize()
        {
            Logger.LogInfo("Initializing Infinite Ammo mod...");
        }

        public void Awake()
        {
            Logger.LogInfo("Initializing Infinite Ammo mod...");
            Harmony harmony = new Harmony("com.Abstractmelon.InfiniteAmmo");
            harmony.PatchAll(typeof(Patches));
            Logger.LogInfo("Harmony patches applied.");
        }

        public override void Unload()
        {
            Logger.LogInfo("Unloading Infinite Ammo mod...");
            Harmony.UnpatchID("com.Abstractmelon.InfiniteAmmo");
        }
    }

    public static class Patches
    {
        [HarmonyPatch(typeof(Weapon), "ammo", MethodType.Getter)]
        public static class InfiniteAmmoPatch
        {
            public static bool Prefix(ref float __result)
            {
                __result = float.MaxValue;
                Logger.LogInfo("Infinite Ammo");
                return false; // Skip original getter
            }
        }
    }
}


using System.Collections;
using Silk;
using Logger = Silk.Logger; // Alias for Silk.Logger to Logger
using HarmonyLib; // Library for runtime method patching

namespace MakeItChristmas
{
    // SilkMod Attribute with with the format: name, authors, mod version, silk version, mod identifier, and networking type
    [SilkMod("Make it Christmas", new[] { "Abstractmelon" }, "1.0.0", "0.5.0", "make-it-christmas", 2)]
    public class MakeItChristmas : SilkMod
    {
        // Called by Silk when Unity loads this mod
        public override void Initialize()
        {
            // Log mod started
            Logger.LogInfo("Initializing Make it Christmas mod...");

            // Create and apply Harmony patches
            Harmony harmony = new Harmony("com.Abstractmelon.MakeItChristmas"); // Create a Harmony instance for patching
            harmony.PatchAll(typeof(Patches)); // Apply all Harmony patches

            // Log mod finished
            Logger.LogInfo("Harmony patches applied.");
        }

        // Called by Silk when the mod is being unloaded, undo what your mod does in `Initialize()`
        public override void Unload()
        {
            Logger.LogInfo("Unloading Make it Christmas mod...");
            Harmony.UnpatchID("com.Abstractmelon.MakeItChristmas");
        }
    }

    public static class Patches
    {
        [HarmonyPatch(typeof(SeasonChecker), nameof(SeasonChecker.IsItChristmas))]
        [HarmonyPrefix]
        public static bool MakeItChristmas(ref bool __result)
        {
            __result = true;
            return false;
        }
    }
}


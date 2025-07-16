using Silk;
using Logger = Silk.Logger;
using HarmonyLib;
using UnityEngine;
using Silk.API;
using System.Collections.Generic;

namespace LongSword
{
    [SilkMod("Long Sword", new[] { "Abstractmelon" }, "1.0.0", "0.6.1", "long-sword", 1)]
    public class LongSword : SilkMod
    {
        public const string ModId = "long-sword";

        private Harmony _harmony;

        public override void Initialize()
        {
            Logger.LogInfo("Initializing Long Sword...");

            CustomWeapon longSwordWeapon = new CustomWeapon("Long Sword", Weapons.WeaponType.ParticleBlade);
            Weapons.AddNewWeapon(longSwordWeapon);
            Weapons.OnInitCompleted += () =>
            {
                ParticleBlade particleBladeComponent = longSwordWeapon.WeaponObject.GetComponent<ParticleBlade>();
                particleBladeComponent.baseSize = new Vector2(10, 100);
            };
        }

        public void Awake()
        {
            Logger.LogInfo("Awake called.");
        }

        public override void Unload()
        {
            Logger.LogInfo("Unloading Long Sword...");
            _harmony.UnpatchSelf();
        }
    }
}

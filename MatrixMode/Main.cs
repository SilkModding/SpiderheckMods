using Silk;
using Logger = Silk.Logger;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace MatrixMode
{
    [SilkMod("Matrix Mode", new[] { "Abstractmelon" }, "1.0.0", "0.7.0", "matrix-mode", 1)]
    public class MatrixMode : SilkMod
    {
        public const string ModId = "matrix-mode";
        private bool _slowMoActive = false;
        public static MatrixMode Instance;

        public override void Initialize()
        {
            Logger.LogInfo("Initializing Matrix Mode Mod...");
            Instance = this;

            var defaultConfig = new Dictionary<string, object>
            {
                { "enabled", true },
                { "slowMotionScale", 0.3f },
                { "triggerKey", "leftShift" }
            };
            Config.LoadModConfig(ModId, defaultConfig);
        }

        public void Awake()
        {
            Logger.LogInfo("Awake called.");
        }

        public void Update()
        {
            if (!Config.GetModConfigValue(ModId, "enabled", true))
                return;

            // Toggle slow-mo on key hold
            if (Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed)
            {
                if (!_slowMoActive)
                {
                    _slowMoActive = true;
                    ActivateSlowMo();
                }
            }
            else if (_slowMoActive)
            {
                _slowMoActive = false;
                DeactivateSlowMo();
            }
        }

        private void ActivateSlowMo()
        {
            float slowScale = Config.GetModConfigValue(ModId, "slowMotionScale", 0.2f);
            if (TimeScaleController.instance != null)
            {
                TimeScaleController.instance.StartEffect(slowScale, 0.05f, 0f);
                if (CameraEffects.instance != null)
                {
                    CameraEffects.instance.DoChromaticAberration(999f, 0.05f);
                }
            }
        }

        private void DeactivateSlowMo()
        {
            if (TimeScaleController.instance != null)
            {
                TimeScaleController.instance.Reset();
            }
        }
    }
}

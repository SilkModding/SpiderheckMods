using Silk;
using Logger = Silk.Logger;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace FPSCounterMod
{
    [SilkMod("FPS Counter", new[] { "Abstractmelon" }, "1.0.0", "0.7.0", "fps-counter", 1)]
    public class FPSCounter : SilkMod
    {
        public const string ModId = "fps-counter";

        private Harmony _harmony;
        private float _deltaTime = 0.0f;
        private float _updateInterval = 0.5f;
        private float _timeSinceUpdate = 0.0f;
        private float _fps = 60.0f;
        private GUIStyle _style;

        public override void Initialize()
        {
            Logger.LogInfo("Initializing FPS Counter Mod...");

            var defaultConfig = new Dictionary<string, object>
            {
                { "enabled", true },
                { "updateInterval", 0.5f },
                { "fontSize", 24 },
                { "positionX", 10 },
                { "positionY", 10 }
            };
            Config.LoadModConfig(ModId, defaultConfig);

            _harmony = new Harmony("com.modder.fpscounter");
            _harmony.PatchAll(typeof(Patches));
        }

        public void Awake()
        {
            Logger.LogInfo("Awake called.");
            
            // Initialize GUI style
            _style = new GUIStyle();
            _style.fontSize = Config.GetModConfigValue(ModId, "fontSize", 24);
            _style.normal.textColor = Color.green;
            
            _updateInterval = Config.GetModConfigValue(ModId, "updateInterval", 0.5f);
        }

        public void Update()
        {
            if (!Config.GetModConfigValue(ModId, "enabled", true))
                return;

            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
            _timeSinceUpdate += Time.unscaledDeltaTime;

            if (_timeSinceUpdate >= _updateInterval)
            {
                _fps = 1.0f / _deltaTime;
                _timeSinceUpdate = 0.0f;
            }
        }

        public void OnGUI()
        {
            if (!Config.GetModConfigValue(ModId, "enabled", true))
                return;

            if (_style == null)
            {
                _style = new GUIStyle();
                _style.fontSize = Config.GetModConfigValue(ModId, "fontSize", 24);
            }

            // Color code based on FPS
            if (_fps >= 60)
                _style.normal.textColor = Color.green;
            else if (_fps >= 30)
                _style.normal.textColor = Color.yellow;
            else
                _style.normal.textColor = Color.red;

            int posX = Config.GetModConfigValue(ModId, "positionX", 10);
            int posY = Config.GetModConfigValue(ModId, "positionY", 10);

            string fpsText = $"FPS: {Mathf.Ceil(_fps)}";
            GUI.Label(new Rect(posX, posY, 200, 50), fpsText, _style);
        }

        public override void Unload()
        {
            Logger.LogInfo("Unloading FPS Counter Mod...");
            _harmony.UnpatchSelf();
        }
    }

    public static class Patches
    {
        // Placeholder for any Harmony patches if needed
    }
}

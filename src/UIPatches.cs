namespace TWGlobalLeaderboards;

using HarmonyLib;
using Steamworks;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[HarmonyPatch]
public static class UIPatches
{
    [HarmonyPostfix] [HarmonyPatch(typeof(ModsConfigHelper), "Start")]
    public static void CreateButtons(ModsConfigHelper __instance)
    {
        LeaderboardDataRequest currentVal = PrefsManager.Get("GlobalLeaderboards.mode", LeaderboardDataRequest.GlobalAroundUser);

        Text txt = __instance.CreateTitle("Leaderboard Mode: " + currentVal.ToString());
        Slider slider = __instance.CreateSlider(0, 2, (int)currentVal);
        slider.onValueChanged.AddListener((val) =>
        {
            int intValue = Mathf.RoundToInt(val);
            var value = (LeaderboardDataRequest)intValue;

            slider.value = intValue;
            txt.text = "Leaderboard Mode: " + value.ToString();
            PrefsManager.Set("GlobalLeaderboards.mode", value);
        });
    }

    // DUVIZZ FUCKING G LOCCK IINNNNN
    extension(ModsConfigHelper help)
    {
        public ButtonController CreateButton(string text = "Example button", string toolTip = "", Action OnClick = null)
        {
            (GameObject obj, UnityEvent onClick) = help.CreateButton(text, toolTip);
            if (OnClick != null)
                onClick.AddListener(() => OnClick());

            return obj.GetComponentInChildren<ButtonController>();
        }

        public Slider CreateSlider(float min = 0f, float max = 100f, float startingValue = 0f)
        {
            (GameObject obj, Slider slider) = help.CreateSlider();
            slider.minValue = min;
            slider.maxValue = max;
            slider.value = startingValue;

            return slider;
        }

        // for some reason this is called createbutton even tho its meant to be create title
        public Text CreateTitle(string text = "Example Title") =>
            help.CreateButton(text).GetComponentInChildren<Text>();
    }
}
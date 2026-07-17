namespace TWGlobalLeaderboards;

using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[HarmonyPatch]
public static class UIPatches
{
    [HarmonyPostfix] [HarmonyPatch(typeof(ModsConfigHelper), "Start")]
    public static void CreateButtons(ModsConfigHelper __instance)
    {
        __instance.CreateTitle("Leaderboard Mode:");
        __instance.CreateSlider();
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
            slider.value = startingValue;
            slider.minValue = min;
            slider.maxValue = max;

            return slider;
        }

        public GameObject CreateTitle(string text = "Example Title") =>
            help.CreateButton(text); // for some reason this is called createbutton even tho its meant to be create title
    }
}
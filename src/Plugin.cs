namespace TWGlobalLeaderboards;

using BepInEx;
using HarmonyLib;

[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
public class Plugin : BaseUnityPlugin
{
    public void Awake()
    {
        ModsConfigHelper.EnableModdedSettings();
        ModsConfigHelper.DestroyDummyButtons();

        new Harmony(PluginInfo.GUID).PatchAll();
    }

    public static class PluginInfo
    {
        public const string GUID = "Bryan_-000-.TWGlobalLeaderboards";
        public const string Name = "TWGlobalLeaderboards";
        public const string Version = "1.0.0";
    }
}
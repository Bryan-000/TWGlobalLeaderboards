namespace TWGlobalLeaderboards;

using BepInEx;

[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
public class Plugin : BaseUnityPlugin
{
    public void Awake()
    {
    }

    public class PluginInfo
    {
        public const string GUID = "Bryan_-000-.TWGlobalLeaderboards";
        public const string Name = "TWGlobalLeaderboards";
        public const string Version = "1.0.0";
    }
}
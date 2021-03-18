using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace CutsceneEnded
{
    [Serializable]
    class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 1;

        public bool ShowToastNotification = true;
        public bool FlashTrayIcon = true;
        public bool AutoActivateWindow = false;
        public bool OnlyMSQ = false;

        [NonSerialized]
        private DalamudPluginInterface pluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;
        }

        public void Save()
        {
            pluginInterface.SavePluginConfig(this);
        }
    }
}

using ImGuiNET;
using System.Numerics;

namespace CutsceneEnded
{
    class ConfigurationGui
    {
        public static void Draw(CutsceneEnded plugin)
        {
            if (!plugin.isConfigOpen)
            {
                plugin.configuration.Save();
                //plugin.pi.Framework.Gui.Chat.Print("[CutsceneEnded] Configuration saved");
                plugin.pi.UiBuilder.OnBuildUi -= plugin.DrawSettings;
                return;
            }
            ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, new Vector2(450, 200));
            if (ImGui.Begin("Cutscene Ended configuration", ref plugin.isConfigOpen))
            {
                ImGui.Text("When cutscene ends do the following if FFXIV is running in background:");
                ImGui.Checkbox("Show tray notification", ref plugin.configuration.ShowToastNotification);
                ImGui.Checkbox("Flash taskbar icon", ref plugin.configuration.FlashTrayIcon);
                ImGui.Checkbox("Bring FFXIV to foreground", ref plugin.configuration.AutoActivateWindow);
                ImGui.Text("Zone locking:");
                ImGui.Checkbox("Only trigger in MSQ roulette dungeons", ref plugin.configuration.OnlyMSQ);
            }
            ImGui.PopStyleVar();
        }
    }
}

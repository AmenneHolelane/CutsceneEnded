using ImGuiNET;
using System.Numerics;

namespace CutsceneEnded
{
    class ConfigurationGui
    {
        static bool windowWasOpen = false;
        public static void Draw(CutsceneEnded plugin)
        {
            if (!plugin.isConfigOpen)
            {
                if (windowWasOpen)
                {
                    plugin.configuration.Save();
                    windowWasOpen = false;
                }
                return;
            }
            windowWasOpen = true;
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

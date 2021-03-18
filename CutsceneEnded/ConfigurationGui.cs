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
            ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, new Vector2(400, 200));
            if (ImGui.Begin("Cutscene Ended configuration", ref plugin.isConfigOpen))
            {
                ImGui.Text("On cutscene end if FFXIV running in background:");
                ImGui.Checkbox("Show toast notification", ref plugin.configuration.ShowToastNotification);
                ImGui.Checkbox("Flash tray icon", ref plugin.configuration.FlashTrayIcon);
                ImGui.Checkbox("Bring FFXIV foreground", ref plugin.configuration.AutoActivateWindow);
                ImGui.Text("Zone locking:");
                ImGui.Checkbox("Only trigger on MSQ roulette dungeons", ref plugin.configuration.OnlyMSQ);
            }
            ImGui.PopStyleVar();
        }
    }
}

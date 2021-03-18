using Dalamud.Game.ClientState;
using Dalamud.Game.Internal;
using Dalamud.Plugin;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CutsceneEnded
{
    class CutsceneEnded : IDalamudPlugin
    {
        public string Name => "CutsceneEnded";

        public DalamudPluginInterface pi;
        public bool isInCutscene = false;
        public Configuration configuration;
        public bool isConfigOpen = false;

        const int CastrumZoneId = 217;
        const int PraetoriumZoneId = 224;

        public void Dispose()
        {
            pi.Framework.OnUpdateEvent -= HandleFrameworkUpdate;
            pi.UiBuilder.OnOpenConfigUi -= OnGuiOpen;
            pi.UiBuilder.OnBuildUi -= DrawSettings;
        }

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            pi = pluginInterface;
            configuration = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            configuration.Initialize(pi);
            pi.Framework.OnUpdateEvent += HandleFrameworkUpdate;
            pi.UiBuilder.OnOpenConfigUi += OnGuiOpen;
        }

        public void DrawSettings()
        {
            ConfigurationGui.Draw(this);
        }

        private void OnGuiOpen(object sender, EventArgs e)
        {
            if (!isConfigOpen)
            {
                isConfigOpen = true;
                pi.UiBuilder.OnBuildUi += DrawSettings;
            }
        }

        private void HandleFrameworkUpdate(Framework framework)
        {
            if (pi.ClientState == null) return;
            var c = pi.ClientState.Condition[ConditionFlag.OccupiedInCutSceneEvent]
                || pi.ClientState.Condition[ConditionFlag.WatchingCutscene78];
            if(isInCutscene && !c && !NativeFunctions.ApplicationIsActivated() &&
                (!configuration.OnlyMSQ || pi.ClientState.TerritoryType == CastrumZoneId || pi.ClientState.TerritoryType == PraetoriumZoneId))
            {
                if (configuration.FlashTrayIcon)
                {
                    var flashInfo = new NativeFunctions.FLASHWINFO
                    {
                        cbSize = (uint)Marshal.SizeOf<NativeFunctions.FLASHWINFO>(),
                        uCount = uint.MaxValue,
                        dwTimeout = 0,
                        dwFlags = NativeFunctions.FlashWindow.FLASHW_ALL |
                                    NativeFunctions.FlashWindow.FLASHW_TIMERNOFG,
                        hwnd = Process.GetCurrentProcess().MainWindowHandle
                    };
                    NativeFunctions.FlashWindowEx(ref flashInfo);
                }
                if(configuration.AutoActivateWindow) SetForegroundWindow(Process.GetCurrentProcess().MainWindowHandle);
                if (configuration.ShowToastNotification)
                {
                    var n = new NotifyIcon
                    {
                        Icon = SystemIcons.Application,
                        Visible = true
                    };
                    n.ShowBalloonTip(int.MaxValue, "", "Cutscene ended", ToolTipIcon.Info);
                    n.BalloonTipClosed += delegate
                    {
                        n.Visible = false;
                        n.Dispose();
                    };
                    n.BalloonTipClicked += delegate
                    {
                        n.Visible = false;
                        n.Dispose();
                    };
                }
            }
            isInCutscene = c;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MauiApplication = Microsoft.Maui.Controls.Application;

namespace MauiApp1
{
    public enum SystemTrayStatus
    {
        Offline = 0,
        Online = 1,
        Warning = 2
    }

    public class SystemTrayService
    {
        private static readonly Dictionary<SystemTrayStatus, string> _systemTrayIconPathsDictionary = new Dictionary<SystemTrayStatus, string>
        {
            { SystemTrayStatus.Offline, "trayicon_offline.ico" },
            { SystemTrayStatus.Online, "trayicon_online.ico" },
            { SystemTrayStatus.Warning, "trayicon_warning.ico" }
        };

        private NotifyIcon _notifyIcon;

        private readonly MainWindowService _mainWindowService;

        public SystemTrayService(
            MainWindowService mainWindowService
        )
        {
            _mainWindowService = mainWindowService;
        }

        public void Initialize(SystemTrayStatus initialStatus)
        {
            CreateNotifyIcon(initialStatus);
        }

        public void SetStatus(SystemTrayStatus status)
        {
            _notifyIcon.Icon = GetIconForStatus(status);
        }

        private void OpenApp(object? sender, EventArgs e)
        {
            _mainWindowService.Show();
        }

        private void ExitApp()
        {
            MauiApplication.Current?.Quit();
        }

        private void CreateNotifyIcon(SystemTrayStatus status)
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = GetIconForStatus(status);
            _notifyIcon.ContextMenuStrip = CreateNotifyIconMenu();
            _notifyIcon.Visible = true;

            _notifyIcon.DoubleClick += OpenApp;
        }

        private Icon GetIconForStatus(SystemTrayStatus status)
        {
            var localPath = _systemTrayIconPathsDictionary[status];

            var binaryPath = AppDomain.CurrentDomain.BaseDirectory;

            return new Icon(Path.Combine(binaryPath, localPath));
        }

        private ContextMenuStrip CreateNotifyIconMenu()
        {
            var contextMenuStrip = new ContextMenuStrip();

            contextMenuStrip.Items.Add("Exit", null, (sender, args) => ExitApp());

            return contextMenuStrip;
        }
    }
}

using MauiApp1;
using Microsoft.UI;
using Microsoft.UI.Windowing;

namespace MauiApp1
{
    public class MainWindowService
    {
        public void Initialize()
        {
            GetWinUIAppWindow().Closing += OnMainWindowClosing;
        }

        public void ResizeWindow(int width, int height)
        {
            var window = GetWinUIAppWindow();
            window.ResizeClient(new Windows.Graphics.SizeInt32(width, height));
        }

        /// <summary>
        /// Called when the main window is being closed.
        /// 
        /// Note we prevent the window from being destroyed and instead hide it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnMainWindowClosing(AppWindow sender, AppWindowClosingEventArgs args)
        {
            Hide();

            args.Cancel = true;
        }

        public void Show()
        {
            AppWindow winuiAppWindow = GetWinUIAppWindow();

            if (winuiAppWindow.IsVisible)
            {
                // Window is possible minimized so just restore it to force it to the front.
                if (winuiAppWindow.Presenter is OverlappedPresenter presenter)
                {
                    presenter.Restore();
                }
            }
            else
            {
                winuiAppWindow.Show();
            }
        }
        public void Hide()
        {
            GetWinUIAppWindow().Hide();
        }

        private AppWindow GetWinUIAppWindow()
        {
            var window = GetMauiAppWindow();

            IntPtr nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WindowId win32WindowsId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
            return AppWindow.GetFromWindowId(win32WindowsId);
        }

        private MauiWinUIWindow GetMauiAppWindow()
        {
            return App.Current.Windows.First().Handler.PlatformView as MauiWinUIWindow;
        }
    }
}

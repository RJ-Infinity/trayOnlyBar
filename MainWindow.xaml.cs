using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ManagedShell;
using ManagedShell.Common.Logging;
using ManagedShell.WindowsTray;
using static ManagedShell.WindowsTray.ExplorerTrayService;

namespace trayOnlyBar {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        readonly TrayService trayService;
        readonly ExplorerTrayService explorerTrayService;
        readonly NotificationArea notificationArea;

        // these colours are aproximations
        readonly Color BackgroundColour = Color.FromArgb(0x99, 0, 0, 0);
        readonly Color HoverColour = Color.FromArgb(26, 248, 248, 248);
        readonly Color SelectColour = Color.FromArgb(37, 254, 254, 254);

        public MainWindow() {
            DataContext = this;

            ShellLogger.Log += ShellLogger_Log;

            InitializeComponent();


            trayService = new TrayService();
            explorerTrayService = new ExplorerTrayService();
            notificationArea = new NotificationArea(NotificationArea.DEFAULT_PINNED, trayService, explorerTrayService);
            notificationArea.Initialize();

            foreach (NotifyIcon icon in notificationArea.AllIcons) {
                trayItems.Children.Add(AddItemToTray(icon));
            }
            notificationArea.AllIcons.CollectionChanged += AllIcons_CollectionChanged;


        }

        private void AllIcons_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            // this is not optimal but it dosent happen often so should be fine
            // TODO: only change the element that changed
            trayItems.Children.Clear();
            foreach (NotifyIcon icon in (ICollectionView)sender) {
                trayItems.Children.Add(AddItemToTray(icon));
            }
        }

        private void ShellLogger_Log(object sender, LogEventArgs e) {
            Console.WriteLine(e.ToString());
        }

        Panel AddItemToTray(NotifyIcon icon) {
            Panel trayIcon = new StackPanel();
            Panel trayIconImgContainer = new StackPanel {
                Margin = new Thickness(3, 11, 3, 11)
            };

            Image image = new Image {
                Source = icon.Icon,
                Height = trayIcon.Height,
            };

            trayIcon.MouseEnter += TrayIcon_MouseEnter;
            trayIcon.MouseLeave += TrayIcon_MouseLeave;
            trayIcon.MouseDown += TrayIcon_MouseDown;
            trayIcon.MouseUp += TrayIcon_MouseUp;

            trayIconImgContainer.Children.Add(image);
            trayIcon.Children.Add(trayIconImgContainer);

            return trayIcon;
        }

        private void TrayIcon_MouseUp(object sender, MouseButtonEventArgs e) {
            Panel trayIcon = (Panel)sender;
            trayIcon.Background = null;
        }

        private void TrayIcon_MouseDown(object sender, MouseButtonEventArgs e) {
            Panel trayIcon = (Panel)sender;
            trayIcon.Background = new SolidColorBrush(SelectColour);
        }

        private void TrayIcon_MouseLeave(object sender, MouseEventArgs e) {
            Panel trayIcon = (Panel)sender;
            trayIcon.Background = null;

        }

        private void TrayIcon_MouseEnter(object sender, MouseEventArgs e) {
            Panel trayIcon = (Panel)sender;
            trayIcon.Background = new SolidColorBrush(HoverColour);
        }

        ~MainWindow() {
            notificationArea.Dispose();
            trayService.Dispose();
            Application.Current.Shutdown();
        }
    }
}

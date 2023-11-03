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


        public MainWindow() {
            DataContext = this;

            ShellLogger.Log += ShellLogger_Log;

            InitializeComponent();


            trayService = new TrayService();
            explorerTrayService = new ExplorerTrayService();
            notificationArea = new NotificationArea(NotificationArea.DEFAULT_PINNED, trayService, explorerTrayService);
            notificationArea.Initialize();

            foreach (NotifyIcon icon in notificationArea.AllIcons) {
                trayItems.Children.Add(new TrayIcon(icon));
            }
            notificationArea.AllIcons.CollectionChanged += AllIcons_CollectionChanged;


        }

        private void AllIcons_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            // this is not optimal but it dosent happen often so should be fine
            // TODO: only change the element that changed
            trayItems.Children.Clear();
            foreach (NotifyIcon icon in (ICollectionView)sender) {
                trayItems.Children.Add(new TrayIcon(icon));
            }
        }

        private void ShellLogger_Log(object sender, LogEventArgs e) {
            Console.WriteLine(e.ToString());
        }

        ~MainWindow() {
            notificationArea.Dispose();
            trayService.Dispose();
            Application.Current.Shutdown();
        }
    }
}

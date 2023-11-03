using ManagedShell.WindowsTray;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace trayOnlyBar {
    internal class TrayIcon: StackPanel {
        // these colours are aproximations
        static readonly Color HoverColour = Color.FromArgb(26, 248, 248, 248);
        static readonly Color SelectColour = Color.FromArgb(37, 254, 254, 254);
        public NotifyIcon Icon;
        public TrayIcon(NotifyIcon icon) {
            Icon = icon;
            Children.Add(new StackPanel {
                Margin = new Thickness(3, 11, 3, 11),
                    Children = {new Image {
                    Source = icon.Icon,
                    Height = Height,
                }},
            });
        }
        protected override void OnMouseUp(MouseButtonEventArgs e) {
            base.OnMouseUp(e);
            Background = null;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e) {
            base.OnMouseDown(e);
            Background = new SolidColorBrush(SelectColour);
        }

        protected override void OnMouseLeave(MouseEventArgs e) {
            base.OnMouseLeave(e);
            Background = null;

        }

        protected override void OnMouseEnter(MouseEventArgs e) {
            base.OnMouseEnter(e);
            Background = new SolidColorBrush(HoverColour);
        }
    }
}

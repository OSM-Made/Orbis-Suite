using OrbisLib2.Common.Database.Types;
using OrbisLib2.General;
using OrbisLib2.Targets;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OrbisNeighborHood.Controls
{
    /// <summary>
    /// Interaction logic for CurrentTargetDisplay.xaml
    /// </summary>
    public partial class CurrentTargetDisplay : UserControl
    {
        public CurrentTargetDisplay()
        {
            InitializeComponent();

            Events.DBTouched += Events_DBTouched;
            Events.TargetStateChanged += Events_TargetStateChanged;
            Events.SelectedTargetChanged += Events_SelectedTargetChanged;

            RefreshTarget();
        }

        private void Events_SelectedTargetChanged(object? sender, SelectedTargetChangedEvent e)
        {
            Dispatcher.Invoke(() => { RefreshTarget(); });
        }

        private void Events_TargetStateChanged(object? sender, TargetStateChangedEvent e)
        {
            Dispatcher.Invoke(() => { RefreshTarget(); });
        }

        private void Events_DBTouched(object? sender, DBTouchedEvent e)
        {
            Dispatcher.Invoke(() => { RefreshTarget(); });
        }

        private void RefreshTarget()
        {
            var CurrentTarget = TargetManager.SelectedTarget;

            if (CurrentTarget != null)
            {
                switch (CurrentTarget.MutableInfo.Status)
                {
                    case TargetStatusType.Offline:
                        CurrentTargetState.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                        CurrentTargetState.ToolTip = "Offline";
                        break;

                    case TargetStatusType.Online:
                        CurrentTargetState.Fill = new SolidColorBrush(Color.FromRgb(255, 140, 0));
                        CurrentTargetState.ToolTip = "Online";
                        break;

                    case TargetStatusType.DebuggingActive:
                    case TargetStatusType.APIAvailable:
                        CurrentTargetState.Fill = new SolidColorBrush(Color.FromRgb(0, 128, 0));
                        CurrentTargetState.ToolTip = "Online & API Available";
                        break;

                    default:
                        CurrentTargetState.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                        CurrentTargetState.ToolTip = "Unknown";
                        break;
                }

                CurrentTargetName.Text = CurrentTarget.IsDefault ? $"★{CurrentTarget.Name}" : CurrentTarget.Name;

                try
                {
                    if (CurrentTarget.MutableInfo.BigAppTitleId == null || !Regex.IsMatch(CurrentTarget.MutableInfo.BigAppTitleId, @"CUSA\d{5}"))
                    {
                        CurrentTargetTitleName.Text = "Unknown Title";
                        CurrentTargetTitleId.Text = "-";
                        CurrentTargetTitleImage.Source = new BitmapImage(new Uri("pack://application:,,,/OrbisNeighborhood;component/Images/DefaultTitleIcon.png"));
                        return;
                    }

                    // Get the title information from the sony tmdb.
                    var title = new TMDB(CurrentTarget.MutableInfo.BigAppTitleId);

                    // Try to parse out the names and icons.
                    var names = title.GetNames();
                    var icons = title.GetIcons();

                    // If we failed any abort now.
                    if (names == null || icons == null)
                    {
                        CurrentTargetTitleName.Text = "Unknown Title";
                        CurrentTargetTitleId.Text = "-";
                        CurrentTargetTitleImage.Source = new BitmapImage(new Uri("pack://application:,,,/OrbisNeighborhood;component/Images/DefaultTitleIcon.png"));
                        return;
                    }

                    // Set the new title info.
                    CurrentTargetTitleName.Text = names[0];
                    CurrentTargetTitleId.Text = title.GetTitleId();
                    CurrentTargetTitleImage.Source = new BitmapImage(new Uri(icons[0]));
                }
                catch
                {
                    CurrentTargetTitleName.Text = "Unknown Title";
                    CurrentTargetTitleId.Text = "-";
                    CurrentTargetTitleImage.Source = new BitmapImage(new Uri("pack://application:,,,/OrbisNeighborhood;component/Images/DefaultTitleIcon.png"));
                }
            }
        }

        private void CurrentTargetName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OrbisLib2.Dialog.SelectTarget.ShowDialog(Window.GetWindow(this));
        }
    }
}

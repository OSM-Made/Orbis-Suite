using OrbisNeighborHood.Controls;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using OrbisLib2.Targets;
using OrbisLib2.General;
using OrbisLib2.Common.Database.Types;
using OrbisLib2.Common;

namespace OrbisNeighborHood.MVVM.View
{
    /// <summary>
    /// Interaction logic for AppListView.xaml
    /// </summary>
    public partial class AppListView : UserControl
    {
        private static readonly string AppCachePath = Path.Combine(Config.OrbisPath, @"AppCache\");

        private static readonly List<string> TitleIdExlusionList = new List<string>()
        {
            "NPXS20108",
            "NPXS20110",
            "NPXS20111",
            "NPXS20118",
            "NPXS20104",
            "NPXS20117",
            "NPXS20107",
            "CUSA00960",
            "CUSA01697",
            "CUSA01780",
            "NPXS20105",
            "NPXS20106",
            "NPXS20114",
            "NPXS20120",
            "NPXS20109",
            "NPXS20112",
            "NPXS20133",
            "NPXS20132",
            "CUSA02012", // Media Player
            "NPXS20979", // Playstation Store

            // Destiny? lol
            "CUSA00219",
            "CUSA00568",
            "CUSA01000"
        };

        private List<AppPanel> PanelList = new List<AppPanel>();

        public AppListView()
        {
            InitializeComponent();

            Events.DBTouched += Events_DBTouched;
            Events.TargetStateChanged += Events_TargetStateChanged;
            // TODO: add event for selected target changing.

            // Refresh the info about the current target.
            RefreshTargetInfo();

            // Set Item source for listbox.
            AppList.ItemsSource = PanelList;

            // Create task to periodically check for app.db changes.
            Task.Run(() => CheckAppDatabase());
        }

        #region Properties

        public string TitleString
        {
            get { return (string)GetValue(TitleStringProperty); }
            set
            {
                SetValue(TitleStringProperty, $"Applications ({value})");
            }
        }

        public static readonly DependencyProperty TitleStringProperty =
            DependencyProperty.Register("TitleString", typeof(string), typeof(AppListView), new PropertyMetadata(string.Empty));

        #endregion

        #region Events / Refresh Target

        private void Events_TargetStateChanged(object? sender, TargetStateChangedEvent e)
        {
            Dispatcher.Invoke(() => { RefreshTargetInfo(); });
        }

        private void Events_DBTouched(object? sender, DBTouchedEvent e)
        {
            Dispatcher.Invoke(() => { RefreshTargetInfo(); });
        }

        private void RefreshTargetInfo()
        {
            var CurrentTarget = TargetManager.SelectedTarget;

            if (CurrentTarget != null)
            {
                // Update title with the new target name.
                TitleString = CurrentTarget.IsDefault ? $"★{CurrentTarget.Name}" : CurrentTarget.Name;

                // Re initialize the application list for the new target.
                // InitAppList();
            }
        }

        #endregion

        private async Task CheckAppDatabase()
        {
            while (true)
            {
                try
                {
                    // Make sure we have a target we can pull the db from.
                    if (TargetManager.Targets.Count <= 0)
                        continue;

                    // Get the current app list.
                    var currentTarget = TargetManager.SelectedTarget;
                    if (currentTarget == null)
                        continue;

                    var appList = await currentTarget.Application.GetAppListAsync();

                    // Check for adding apps.
                    Parallel.ForEach(appList, async app =>
                    {
                        var currentTarget = TargetManager.SelectedTarget;
                        

                        // Make sure the titleId format is correct. Helps weed out bad entries and folders.
                        if (!Regex.IsMatch(app.TitleId, @"[a-zA-Z]{4}\d{5}"))
                            return;

                        // Skip the Destiny entries that just exist for some reason even after a restore?... lol
                        if ((app.TitleId.Equals("CUSA00219") || app.TitleId.Equals("CUSA00568") || app.TitleId.Equals("CUSA01000")) && app.ContentSize <= 0)
                            return;

                        // Skip some that aren't technically an app.
                        if (TitleIdExlusionList.Contains(app.TitleId))
                            return;

                        // Weed out some more bad entries created by default.
                        if (app.TitleName.Length <= 2)
                            return;

                        // Make sure only add ones with a category.
                        if (app.UICategory.Length <= 0 || app.Category.Length <= 0)
                            return;

                        // Directory to cache stuff for app.
                        string currentAppPath = Path.Combine(AppCachePath, app.TitleId);

                        // Create Directory for current app.
                        if (!Directory.Exists(currentAppPath))
                        {
                            Directory.CreateDirectory(currentAppPath);
                        }

                        // Cache icon0.png for app if we have not already.
                        if (!File.Exists(Path.Combine(currentAppPath, "icon0.png")) && !string.IsNullOrEmpty(app.MetaDataPath) && currentTarget.MutableInfo.Status >= TargetStatusType.APIAvailable)
                        {
                            var file = await currentTarget.GetFile($"{app.MetaDataPath}/icon0.png");
                            if (file != null && file.Length > 0) 
                            {
                                File.WriteAllBytes(Path.Combine(currentAppPath, "icon0.png"), file);
                            }
                        }

                        // Fetch the App version.
                        var appVersion = currentTarget.Application.GetAppInfoString(app.TitleId, "APP_VER");

                        Dispatcher.Invoke(() =>
                        {
                            var pannel = PanelList.Find(x => x._App.TitleId == app.TitleId);
                            if(pannel != null)
                            {
                                pannel.Update(app, appVersion);
                            }
                            else
                            {
                                PanelList.Add(new AppPanel(app, appVersion));
                            }
                        });
                    });

                    // Check to remove apps.
                    Parallel.ForEach(PanelList, panel =>
                    {
                        var app = appList.Find(x => x.TitleId == panel._App.TitleId);
                        if(app == null)
                        {
                            PanelList.Remove(panel);
                        }
                    });

                    // Update view.
                    Dispatcher.Invoke(() =>
                    {
                        AppList.Items.Refresh();
                    });

                    await Task.Delay(2000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}

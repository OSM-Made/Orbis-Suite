using OrbisNeighborHood.Core;
using OrbisNeighborHood.MVVM.ViewModel.SubView;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace OrbisNeighborHood.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        // Commands
        public RelayCommand DashboardViewCommand { get; set; }

        public RelayCommand TargetViewCommand { get; set; }

        public RelayCommand AppListViewCommand { get; set; }

        public RelayCommand SettingsViewCommand { get; set; }

        // View Models
        public DashboardViewModel DashboardHomeVM { get; set; }

        public TargetViewModel TargetVM { get; set; }

        public AppListViewModel AppListVM { get; set; }

        public SettingsViewModel SettingsVM { get; set; }


        //SubViews
        public RelayCommand AddTargetViewCommand { get; set; }

        public AddTargetViewModel AddTargetVM { get; set; }

        public RelayCommand EditTargetViewCommand { get; set; }

        public EditTargetViewModel EditTargetVM { get; set; }


        private object? _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public static MainViewModel? Instance { get; private set; }


        public MainViewModel()
        {
            Instance = this;

            // MainViews
            DashboardHomeVM = new DashboardViewModel();
            TargetVM = new TargetViewModel();
            AppListVM = new AppListViewModel();
            SettingsVM = new SettingsViewModel();

            // Sub Views
            AddTargetVM = new AddTargetViewModel();
            EditTargetVM = new EditTargetViewModel();

            // Set Current View.
            CurrentView = DashboardHomeVM;

            //Set up relay commands.
            DashboardViewCommand = new RelayCommand(o =>
            {
                CurrentView = DashboardHomeVM;
            });

            TargetViewCommand = new RelayCommand(o =>
            {
                CurrentView = TargetVM;
            });

            AppListViewCommand = new RelayCommand(o =>
            {
                CurrentView = AppListVM;
            });

            SettingsViewCommand = new RelayCommand(o =>
            {
                CurrentView = SettingsVM;
            });

            // Sub Views Relay Commands
            AddTargetViewCommand = new RelayCommand(o =>
            {
                CurrentView = AddTargetVM;
            });

            EditTargetViewCommand = new RelayCommand(o =>
            {
                CurrentView = EditTargetVM;
            });
        }
    }
}

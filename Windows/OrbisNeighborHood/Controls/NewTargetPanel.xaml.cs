using OrbisNeighborHood.MVVM.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OrbisNeighborHood.Controls
{
    /// <summary>
    /// Interaction logic for NewTargetView.xaml
    /// </summary>
    public partial class NewTargetPanel : UserControl
    {
        public event EventHandler<RoutedEventArgs>? TargetChanged;

        public NewTargetPanel()
        {
            InitializeComponent();
        }

        private void AddTargetElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(MainViewModel.Instance != null)
            {
                MainViewModel.Instance.AddTargetVM.TargetChanged += AddTargetVM_TargetChanged;
                MainViewModel.Instance.CurrentView = MainViewModel.Instance.AddTargetVM;
            }  
        }

        private void AddTargetVM_TargetChanged(object? sender, RoutedEventArgs e)
        {
            TargetChanged?.Invoke(sender, e);
        }
    }
}

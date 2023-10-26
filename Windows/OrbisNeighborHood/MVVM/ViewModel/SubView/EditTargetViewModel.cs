using OrbisLib2.Common.Database;
using OrbisLib2.Targets;
using System;
using System.Windows;

namespace OrbisNeighborHood.MVVM.ViewModel.SubView
{
    internal class EditTargetViewModel
    {
        public EditTargetViewModel()
        {

        }

        #region Events

        public event EventHandler<RoutedEventArgs>? TargetChanged;

        public void DoTargetChanged()
        {
            TargetChanged?.Invoke(this, new RoutedEventArgs());
        }

        #endregion

        #region Dependency Properties

        public SavedTarget CurrentTarget { get; set; }

        public object CallingVM { get; set; }

        #endregion
    }
}

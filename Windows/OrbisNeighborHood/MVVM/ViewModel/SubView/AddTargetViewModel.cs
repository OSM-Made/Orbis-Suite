using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OrbisNeighborHood.MVVM.ViewModel.SubView
{
    internal class AddTargetViewModel
    {
        public event EventHandler<RoutedEventArgs>? TargetChanged;

        public AddTargetViewModel()
        {
            
        }

        public void DoTargetChanged()
        {
            TargetChanged?.Invoke(this, new RoutedEventArgs());
        }
    }
}

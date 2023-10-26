using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetupBA.MVVM.ViewModel
{
    public class SummaryViewModel
    {
        public SummaryViewModel(MainViewModel mainViewModel)
        {
            MainVM = mainViewModel;
        }

        public MainViewModel MainVM { get; set; }
    }
}

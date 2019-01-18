using QLHangHoa.ViewModel;
using System;
using System.Collections.Generic;
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

namespace QLHangHoa.CustomUserControl
{
    /// <summary>
    /// Interaction logic for customUC.xaml
    /// </summary>
    public partial class customUC : UserControl
    {
        public CustomUserControlViewModel viewmodel { get; set; }
        public customUC()
        {
            InitializeComponent();
            this.DataContext = viewmodel = new CustomUserControlViewModel();
        }
    }
}

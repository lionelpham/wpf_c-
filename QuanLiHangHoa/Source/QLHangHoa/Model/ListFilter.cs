using QLHangHoa.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLHangHoa.Model
{
    public class Filter : BaseViewModel
    {
        private string _day;
        public string day { get=> _day; set { _day = value;OnPropertyChanged(); }}

        private string _month;
        public string month { get => _month; set { _month = value; OnPropertyChanged(); } }

        private string _year;
        public string year { get => _year; set { _year = value; OnPropertyChanged(); } }
    }
}

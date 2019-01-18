using QLHangHoa.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLHangHoa.Model
{
    public class HangTonKho : BaseViewModel
    {
        private SanPham _sanPham;
        public SanPham sanPham { get => _sanPham; set { _sanPham = value; OnPropertyChanged(); } }
        private int _count;
        public int count { get=>_count; set { _count = value;OnPropertyChanged(); }}

    }
}

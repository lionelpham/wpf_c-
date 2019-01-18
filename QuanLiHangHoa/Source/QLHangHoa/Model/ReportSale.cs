using QLHangHoa.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLHangHoa.Model
{
    public class ReportSale : BaseViewModel
    {
     
        private int? _soLuongBan;
        public int? soLuongBan { get => _soLuongBan; set { _soLuongBan = value; OnPropertyChanged(); } }

        private DateTime? _ngayBan;
        public DateTime? ngayBan { get => _ngayBan; set { _ngayBan = value; OnPropertyChanged(); } }

        private string _MaLoai;
        public string MaLoai { get => _MaLoai; set { _MaLoai = value; OnPropertyChanged(); } }

        private string _TenLoai;
        public string TenLoai { get => _TenLoai; set { _TenLoai = value; OnPropertyChanged(); } }

        private int? _tongTien;
        public int? tongTien { get => _tongTien; set { _tongTien = value; OnPropertyChanged(); } }
    }
}

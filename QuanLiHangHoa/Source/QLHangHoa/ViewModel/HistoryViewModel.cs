using QLHangHoa.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QLHangHoa.ViewModel
{
    public class HistoryViewModel : BaseViewModel
    {
        private ObservableCollection<HoaDon> _ListHD;
        public ObservableCollection<HoaDon> ListHD { get => _ListHD; set { _ListHD = value; OnPropertyChanged(); } }

        private List<HoaDon> _ListHDFix;
        public List<HoaDon> ListHDFix { get => _ListHDFix; set { _ListHDFix = value; OnPropertyChanged(); } }

        public ICommand EditCommandHD { get; set; }
        public ICommand AddCommandHD { get; set; }
        public ICommand NextCommandd { get; set; }
        public ICommand PrevCommandd { get; set; }

        private int _PageNumberCurrentt;
        public int PageNumberCurrentt { get => _PageNumberCurrentt; set { _PageNumberCurrentt = value; OnPropertyChanged(); } }


        private HoaDon _SelectedItemHD;
        public HoaDon SelectedItemHD
        {
            get => _SelectedItemHD;
            set
            {
                _SelectedItemHD = value;
                OnPropertyChanged();
                if (SelectedItemHD != null)
                {
                    MaHoaDon = SelectedItemHD.MaHoaDon;
                    HoTen = SelectedItemHD.HoTen;
                    SoDienThoai = SelectedItemHD.SoDienThoai;
                    NgayBan = SelectedItemHD.NgayBan;
                    HinhThucThanhToan = SelectedItemHD.HinhThucThanhToan;
                    ChuThich = SelectedItemHD.ChuThich;
                    SoLuongBan = SelectedItemHD.SoLuongBan;
                    MaSp = SelectedItemHD.MaSp;
                }
                //OnPropertyChanged();
            }
        }


        public bool admin = false;
        private string _MaHoaDon;
        public string MaHoaDon { get => _MaHoaDon; set { _MaHoaDon = value; OnPropertyChanged(); } }

        private string _HoTen;
        public string HoTen { get => _HoTen; set { _HoTen = value; OnPropertyChanged(); } }

        private int? _SoDienThoai;
        public int? SoDienThoai { get => _SoDienThoai; set { _SoDienThoai = value; OnPropertyChanged(); } }

        private DateTime? _NgayBan;
        public DateTime? NgayBan { get => _NgayBan; set { _NgayBan = value; OnPropertyChanged(); } }

        private string _HinhThucThanhToan;
        public string HinhThucThanhToan { get => _HinhThucThanhToan; set { _HinhThucThanhToan = value; OnPropertyChanged(); } }

        //tình trạng
        private string _ChuThich;
        public string ChuThich { get => _ChuThich; set { _ChuThich = value; OnPropertyChanged(); } }

        private string _MaSp;
        public string MaSp { get => _MaSp; set { _MaSp = value; OnPropertyChanged(); } }

        private int? _SoLuongBan;
        public int? SoLuongBan { get => _SoLuongBan; set { _SoLuongBan = value; OnPropertyChanged(); } }

        public HistoryViewModel()
        {
            LoginWindow loginWindow = new LoginWindow();
            var login = loginWindow.DataContext as LoginViewModel;
            admin = login.isAdmin;
            ListHDFix = new List<HoaDon>();
            ListHD = new ObservableCollection<HoaDon>(DataProvider.Instace.Data.HoaDons);
            PageNumberCurrentt = 1;
            var k = ListHD.Skip((PageNumberCurrentt-1) * 8).Take(8);
            ListHDFix = k.ToList();
            //Edit command
            EditCommandHD = new RelayCommand<object>((p) =>
            {
                if (admin == false)
                    return false;
                if (SelectedItemHD.MaHoaDon != MaHoaDon || SelectedItemHD.MaSp != MaSp || string.IsNullOrEmpty(HoTen) || string.IsNullOrEmpty(MaSp) || SelectedItemHD.SoLuongBan != SoLuongBan)
                    return false;

                return true;
            },(p) =>
           {


               var hd = DataProvider.Instace.Data.HoaDons.Where(z => z.MaHoaDon == SelectedItemHD.MaHoaDon).SingleOrDefault();

               hd.MaHoaDon = MaHoaDon;
               hd.HoTen = HoTen;
               hd.HinhThucThanhToan = HinhThucThanhToan;
               hd.MaSp = MaSp;
               hd.SoDienThoai = SoDienThoai;
               hd.SoLuongBan = SoLuongBan;
               hd.ChuThich = ChuThich;
               hd.NgayBan = NgayBan;
               // var sp1 = new SanPham() { TenSp = TenSp, MaSp = MaSp, NgayNhap = NgayNhap, SoLuongSp = SoLuongSp, MaLoai = MaLoai, GiaSp = GiaSp, HoaDons = null };


               //DataProvider.Instace.Data.SanPhams.Add(sp1);
               DataProvider.Instace.Data.SaveChanges();

    
               OnPropertyChanged("ListHD");

               //TonKhoList.Add(updateTonKho);
               MessageBox.Show("Chỉnh sửa thành công");
               MaSp = "";
               HoTen = "";
               HinhThucThanhToan = "";
               SoDienThoai = 0;
               SoLuongBan = 0;
               ChuThich = "";
               NgayBan = null;
               MaHoaDon = "";
           });

            AddCommandHD = new RelayCommand<object>(p =>
            {
                var q = DataProvider.Instace.Data.HoaDons.Where(s => s.MaHoaDon == MaHoaDon);
                var l = DataProvider.Instace.Data.SanPhams.Where(s => s.MaSp == MaSp);
                if (string.IsNullOrEmpty(MaSp)|| string.IsNullOrEmpty(MaHoaDon) || string.IsNullOrEmpty(HoTen) || q.Count() > 0 || l.Count() == 0 ||SoLuongBan == 0)
                    return false;
                return true;
            }, p =>
            {
                HoaDon hd1 = new HoaDon()
                {
                    MaHoaDon = MaHoaDon,
                    HoTen = HoTen,
                    HinhThucThanhToan = HinhThucThanhToan,
                    MaSp = MaSp,
                    SoDienThoai = SoDienThoai,
                    SoLuongBan = SoLuongBan,
                    ChuThich = ChuThich,
                    NgayBan = NgayBan,
                    SanPham = null
                };
                DataProvider.Instace.Data.HoaDons.Add(hd1);
                DataProvider.Instace.Data.SaveChanges();
                ListHD.Add(hd1);
                MessageBox.Show("Thêm thành công");
                OnPropertyChanged("ListHD");
                MaSp = "";
                HoTen = "";
                HinhThucThanhToan = "";
                SoDienThoai = 0;
                SoLuongBan = 0;
                ChuThich = "";
                NgayBan = null;
                MaHoaDon = "";
            });

            NextCommandd = new RelayCommand<object>((p) => {
                if (PageNumberCurrentt > ListHD.Count() / 8)
                    return false;
                return true;
            },(p) =>
            {
                PageNumberCurrentt = PageNumberCurrentt + 1;
                var z= ListHD.Skip((PageNumberCurrentt-1) * 8).Take(8);
                ListHDFix = z.ToList();
            });

            PrevCommandd = new RelayCommand<object>((p) => {
                if (PageNumberCurrentt <= 1)
                    return false;
                
                    return true;
            },(p) =>
            {
                PageNumberCurrentt--;
                var t = ListHD.Skip((PageNumberCurrentt-1) * 8).Take(8);
                ListHDFix = t.ToList();
            });
        }
    }
}


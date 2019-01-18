using QLHangHoa.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace QLHangHoa.ViewModel
{

    public class MainViewModel : BaseViewModel
    {
        //get MaLoai in LoaiSanPham
        //int sizePage = 9;

        private List<SanPham> _SanPhamSort;
        public List<SanPham> SanPhamSort { get => _SanPhamSort; set { _SanPhamSort = value; OnPropertyChanged(); } }

        private List<HangTonKho> _TonKhoList;
        public List<HangTonKho> TonKhoList { get => _TonKhoList; set { _TonKhoList = value; OnPropertyChanged(); } }

        private List<HangTonKho> _TonKhoFix;
        public List<HangTonKho> TonKhoFix { get => _TonKhoFix; set { _TonKhoFix = value; OnPropertyChanged(); } }

        public bool isLoaded = false;
        public bool admin = false;
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand LogOutWindowCommand { get; set; }
        public ICommand TextChangedSearch { get; set; }
        public ICommand TransitionCommand { get; set; }
        public ICommand statictisCommand { get; set; }
        public ICommand ManageAccountCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand DragEnterCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand PrevCommand { get; set; }

        public ICommand AndroidSort { get; set; }
        public ICommand IphoneSort { get; set; }
        public ICommand PKSort { get; set; }
        public ICommand DPSort { get; set; }
        public ICommand AsSort { get; set; }
        public ICommand DsSort { get; set; }
        public ICommand SLDsSort { get; set; }
        public ICommand SLAsSort { get; set; }
        public ICommand AllCommand { get; set; }
        
        //get SelectedItem

        private string _SelectedLoaiSpM;
        public string SelectedLoaiSpM
        {
            get => _SelectedLoaiSpM;
            set
            {
                _SelectedLoaiSpM = value;
                OnPropertyChanged();


            }
        }

        private HangTonKho _SelectedItem;
        public HangTonKho SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    MaSp = SelectedItem.sanPham.MaSp;
                    TenSp = SelectedItem.sanPham.TenSp;
                    GiaSp = SelectedItem.sanPham.GiaSp;
                    SoLuongSp = SelectedItem.count;
                    NgayNhap = SelectedItem.sanPham.NgayNhap;
                    MaLoai = SelectedItem.sanPham.MaLoai;
                    SelectedLoaiSpM = SelectedItem.sanPham.MaLoai;
                }
                //OnPropertyChanged();
            }
        }
        private List<string> _LoaiSanPham;
        public List<string> LoaiSanPham { get => _LoaiSanPham; set { _LoaiSanPham = value; OnPropertyChanged(); } }
        private int _PageNumberCurrent;
        public int PageNumberCurrent { get => _PageNumberCurrent; set { _PageNumberCurrent = value; OnPropertyChanged(); } }

        private string _txtSearch;
        public string txtSearch { get => _txtSearch; set { _txtSearch = value; OnPropertyChanged(); } }

        private string _MaLoaiSP;
        public string MaLoaiSP { get => _MaLoaiSP; set { _MaLoaiSP = value; OnPropertyChanged(); } }

        private string _MaLoai;
        public string MaLoai { get => _MaLoai; set { _MaLoai = value; OnPropertyChanged(); } }

        private int? _GiaSp;
        public int? GiaSp { get => _GiaSp; set { _GiaSp = value; OnPropertyChanged(); } }

        private string _MaSp;
        public string MaSp { get => _MaSp; set { _MaSp = value; OnPropertyChanged(); } }

        private string _TenSp;
        public string TenSp { get => _TenSp; set { _TenSp = value; OnPropertyChanged(); } }

        private DateTime? _NgayNhap;
        public DateTime? NgayNhap { get => _NgayNhap; set { _NgayNhap = value; OnPropertyChanged(); } }

        private int _SoLuongSp;
        public int SoLuongSp { get => _SoLuongSp; set { _SoLuongSp = value; OnPropertyChanged(); } }

        public MainViewModel()
        {

            TonKhoList = new List<HangTonKho>();
            TonKhoFix = new List<HangTonKho>();
            //only one
            var x = from q in DataProvider.Instace.Data.LoaiSanPhams select q.MaLoai;
            LoaiSanPham = x.ToList<string>();

            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                isLoaded = true;

                if (p == null)
                    return;

                //App.Current.MainWindow.Hide(); == dùng parameter từ mainWindow truyền vào để ẩn main window
                p.Hide();

                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();

                var login = loginWindow.DataContext as LoginViewModel;
                admin = login.isAdmin;
                if (login.isLogined)
                {
                    p.Show();
                    LoadDataSanPham();
                }
                else
                {
                    p.Close();
                }
            });

            TextChangedSearch = new RelayCommand<object>((p) =>
            {
                if (txtSearch.Length == 0) { return false; }
                return true;
            }, (p) =>
            {
                TonKhoList.Clear();

                var spList = DataProvider.Instace.Data.SanPhams.Where(f => f.TenSp.Contains(txtSearch));
                if (txtSearch.Length < 4 && txtSearch.Length > 0)
                {
                    spList = DataProvider.Instace.Data.SanPhams.Where(f => f.MaSp.Contains(txtSearch));
                }
                foreach (var item in spList)
                {
                    //Xóa mã loại = null
                    if (item.MaLoai != null && item.SoLuongSp > 0)
                    {
                        var hd = DataProvider.Instace.Data.HoaDons.Where(s => s.MaSp == item.MaSp);
                        int count = (int)item.SoLuongSp;
                        int sum = 0;
                        foreach (var j in hd)
                        {
                            if (j.MaSp == item.MaSp)
                            {
                                sum += (int)j.SoLuongBan;

                            }
                        }
                        count = count - sum;
                        HangTonKho hangTonKho = new HangTonKho();
                        hangTonKho.sanPham = item;
                        hangTonKho.count = count;

                        TonKhoList.Add(hangTonKho);
                    }
                    else
                    {
                        //sp đã bị xóa hoặc hết hàng
                    }
                    PageNumberCurrent = 1;
                    var q = TonKhoList.Skip((PageNumberCurrent - 1) * 9).Take(9);
                    TonKhoFix = q.ToList();
                }

                OnPropertyChanged("TonKhoList");
            });

            TransitionCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                HistoryTransition Trans = new HistoryTransition();
                Trans.ShowDialog();
            });

            statictisCommand = new RelayCommand<object>((p) =>
            {
                if (admin == false)
                    return false;
                return true;
            }, (p) =>
            {
            Statictis statisticWindow = new Statictis();
            statisticWindow.ShowDialog();
            });

            ManageAccountCommand = new RelayCommand<object>((p) => {
                if (admin == false)
                    return false;
                return true;
            }, (p) =>
            {
                ManageAccount manageAcc = new ManageAccount();
                manageAcc.ShowDialog();
            });

            LogOutWindowCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                App.Current.MainWindow.Hide();
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
                App.Current.MainWindow.Close();
                
            });
            AddCommand = new RelayCommand<object>((p) =>
            {
                return true;
            },
            (p) =>
            {
                TonKhoList.Clear();
                TonKhoFix.Clear();
                LoadDataSanPham();
                DataProvider.Instace.Data.SaveChanges();
            });


            AddCommand = new RelayCommand<object>((p) =>
            {
                var q = DataProvider.Instace.Data.SanPhams.Where(s => s.MaSp == MaSp);
                var t = DataProvider.Instace.Data.SanPhams.Where(s => s.TenSp == TenSp);
                var z = DataProvider.Instace.Data.SanPhams.Where(c => c.SoLuongSp == -1);
                if (string.IsNullOrEmpty(MaSp) || string.IsNullOrEmpty(TenSp) || (q.Count() > 0 || t.Count() > 0) || z.Count() <= 0)
                    return false;

                return true;
            },
            (p) =>
            {
                int count = SoLuongSp;
                var sp1 = new SanPham() { TenSp = TenSp, MaSp = MaSp, NgayNhap = NgayNhap, SoLuongSp = SoLuongSp, MaLoai = MaLoai, GiaSp = GiaSp, HoaDons = null };
                var updateTonKho = new HangTonKho();

                DataProvider.Instace.Data.SanPhams.Add(sp1);
                TonKhoList.Clear();
                TonKhoFix.Clear();
                LoadDataSanPham();
                DataProvider.Instace.Data.SaveChanges();
                /*co edit*/
                //updateTonKho.sanPham = sp1;
                //updateTonKho.count = count;
                //TonKhoList.Add(updateTonKho);
                MessageBox.Show("Thêm thành công");
                MaSp = "";
                TenSp = "";
                SoLuongSp = 0;
                GiaSp = 0;
                MaLoaiSP = "";
                NgayNhap = null;

            });
            //Update lai ton kho
            //Edit command
            EditCommand = new RelayCommand<object>((p) =>
            {
                var q = DataProvider.Instace.Data.HoaDons.Where(s => s.MaSp == MaSp).Select(c => c.SoLuongBan);
                var count1 = q.Sum();
                if (admin == false)
                    return false;
                if (SelectedItem.sanPham.MaSp != MaSp || string.IsNullOrEmpty(MaSp) || string.IsNullOrEmpty(TenSp) || SelectedItem == null || count1 - SoLuongSp > 0)
                    return false;
                return true;
            }, (p) =>
            {
                var spList = DataProvider.Instace.Data.SanPhams;
                int? count = 0;
                foreach (var item in spList)
                {
                    //Xóa mã loại = null
                    if (item.MaLoai != null && item.SoLuongSp > 0)
                    {
                        var hd = DataProvider.Instace.Data.HoaDons.Where(s => s.MaSp == item.MaSp);
                        count = (int)item.SoLuongSp;
                        int sum = 0;
                        foreach (var j in hd)
                        {
                            if (j.MaSp == item.MaSp)
                            {
                                sum += (int)j.SoLuongBan;

                            }
                        }
                        count = count - sum;

                    }
                }
                var sp2 = DataProvider.Instace.Data.SanPhams.Where(z => z.MaSp == SelectedItem.sanPham.MaSp).SingleOrDefault();

                sp2.TenSp = TenSp;
                sp2.MaSp = MaSp;
                sp2.NgayNhap = NgayNhap;
                sp2.SoLuongSp = count;
                sp2.MaLoai = SelectedLoaiSpM;
                sp2.GiaSp = GiaSp;
                // var sp1 = new SanPham() { TenSp = TenSp, MaSp = MaSp, NgayNhap = NgayNhap, SoLuongSp = SoLuongSp, MaLoai = MaLoai, GiaSp = GiaSp, HoaDons = null };


                //DataProvider.Instace.Data.SanPhams.Add(sp1);
                DataProvider.Instace.Data.SaveChanges();
                //chưa cap nhat lai khi sua

                SelectedItem.sanPham.TenSp = TenSp;
                SelectedItem.sanPham.MaSp = MaSp;
                SelectedItem.sanPham.NgayNhap = NgayNhap;
                SelectedItem.sanPham.SoLuongSp = SoLuongSp;
                SelectedItem.sanPham.MaLoai = SelectedLoaiSpM;
                SelectedItem.sanPham.GiaSp = GiaSp;
                SelectedItem.count = SoLuongSp;
                //LoadDataSanPham();

                //TonKhoList.Add(updateTonKho);
                MessageBox.Show("Chỉnh sửa thành công");
                MaSp = "";
                TenSp = "";
                SoLuongSp = 0;
                GiaSp = 0;
                MaLoai = "";
                NgayNhap = null;
                //SelectedItem.sanPham.MaLoai = "";
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                var q = DataProvider.Instace.Data.SanPhams.Where(s => s.MaSp == MaSp);
                var t = DataProvider.Instace.Data.SanPhams.Where(s => s.TenSp == TenSp);
                if (admin == false)
                    return false;
                if (SelectedItem.sanPham.SoLuongSp < 0)
                    return false;
                return true;
            }, (p) =>
            {
                SelectedItem.sanPham.SoLuongSp = -1;
                DataProvider.Instace.Data.SaveChanges();

            });

            NextCommand = new RelayCommand<object>((p) =>
            {
                if (PageNumberCurrent > (int)TonKhoList.Count() / 9)
                    return false;
                return true;
            }, (p) =>
             {
                 PageNumberCurrent = PageNumberCurrent + 1;
                 var q = TonKhoList.Skip((PageNumberCurrent - 1) * 9).Take(9);
                 TonKhoFix = q.ToList();
             });
            PrevCommand = new RelayCommand<object>((p) =>
            {
                if (PageNumberCurrent <= 1)
                    return false;
                else
                    return true;
            }, (p) =>
             {
                 PageNumberCurrent--;
                 var q = TonKhoList.Skip((PageNumberCurrent - 1) * 9).Take(9);
                 TonKhoFix = q.ToList();
             });

            AndroidSort = new RelayCommand<object>((p) =>
            {
                var q = TonKhoList.Where(t => t.sanPham.MaLoai.Equals("AD001") && t.sanPham.SoLuongSp > 0);
                if (q.Count() <= 0)
                    return false;
                return true;
            }, (p) =>
             {
                 var q = TonKhoList.Where(t => t.sanPham.MaLoai.Equals("AD001") && t.sanPham.SoLuongSp > 0);
                 PageNumberCurrent = 1;
                 TonKhoList = q.ToList();
                 var i = TonKhoList.Skip((PageNumberCurrent - 1) * 9).Take(9);
                 TonKhoFix = i.ToList();
             });

            IphoneSort = new RelayCommand<object>((p) =>
            {
                var q = TonKhoList.Where(t => t.sanPham.MaLoai.Equals("AP001") && t.sanPham.SoLuongSp > 0);
                if (q.Count() <= 0)
                    return false;
                else
                    return true;
            }, (p) =>
             {
                 var q = TonKhoList.Where(t => t.sanPham.MaLoai.Equals("AP001") && t.sanPham.SoLuongSp > 0);
                 PageNumberCurrent = 1;
                 TonKhoList = q.ToList();
                 var i = TonKhoList.Skip((PageNumberCurrent - 1) * 9).Take(9);
                 TonKhoFix = i.ToList();
             });

            PKSort = new RelayCommand<object>((p) =>
            {
                var q = TonKhoList.Where(t => t.sanPham.MaLoai.Equals("PK001") && t.sanPham.SoLuongSp > 0);
                if (q.Count() <= 0)
                    return false;
                else
                    return true;
            }, (p) =>
             {
                 var q = TonKhoList.Where(t => t.sanPham.MaLoai.Equals("PK001") && t.sanPham.SoLuongSp > 0);

                 TonKhoList = q.ToList();
                 PageNumberCurrent = 1;
                 var i = TonKhoList.Skip((PageNumberCurrent - 1) * 9).Take(9);
                 TonKhoFix = i.ToList();
             });

            DPSort = new RelayCommand<object>((p) =>
            {
                var q = TonKhoList.Where(t => t.sanPham.MaLoai.Equals("PK002") && t.sanPham.SoLuongSp > 0);
                if (q.Count() <= 0)
                    return false;
                else
                    return true;
            }, (p) =>
             {
                 var q = TonKhoList.Where(t => t.sanPham.MaLoai.Equals("PK002") && t.sanPham.SoLuongSp > 0);
                 TonKhoList = q.ToList();
                 PageNumberCurrent = 1;
                 var i = TonKhoList.Skip((PageNumberCurrent - 1) * 9).Take(9);
                 TonKhoFix = i.ToList();
             });
            //giá tăng
            AsSort = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
             {
                 var q = TonKhoList.OrderBy(s => s.sanPham.GiaSp);
                 TonKhoList = q.ToList();
                 PageNumberCurrent = 1;
                 var i = TonKhoList.Skip((PageNumberCurrent - 1) * 9).Take(9);
                 TonKhoFix = i.ToList();
             });
            //giá giảm
            DsSort = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
             {
                 var q = TonKhoList.OrderByDescending(s => s.sanPham.GiaSp);
                 TonKhoList = q.ToList();
                 PageNumberCurrent = 1;
                 var i = TonKhoList.Skip((PageNumberCurrent - 1) * 9).Take(9);
                 TonKhoFix = i.ToList();
             });
            //sl giảm
            SLDsSort = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
             {
                 var q = TonKhoList.OrderByDescending(s => s.count);
                 TonKhoList = q.ToList();
                 PageNumberCurrent = 1;
                 var i = TonKhoList.Skip((PageNumberCurrent - 1) * 9).Take(9);
                 TonKhoFix = i.ToList();
             });
            //sl tăng
            SLAsSort = new RelayCommand<object>((p) =>
            {

                return true;
            }, (p) =>
             {
                 var q = TonKhoList.OrderBy(s => s.count);
                 TonKhoList = q.ToList();
                 PageNumberCurrent = 1;
                 var i = TonKhoList.Skip((PageNumberCurrent - 1) * 9).Take(9);
                 TonKhoFix = i.ToList();
             });

            AllCommand = new RelayCommand<object>((p) =>
            {

                return true;
            }, (p) =>
             {
                 TonKhoList.Clear();
                 TonKhoFix.Clear();
                 LoadDataSanPham();
             });

        }
        void LoadDataSanPham()
        {

            var spList = DataProvider.Instace.Data.SanPhams;

            foreach (var item in spList)
            {

                if (item.MaLoai != null && item.SoLuongSp >= 0)
                {
                    var hd = DataProvider.Instace.Data.HoaDons.Where(s => s.MaSp == item.MaSp);
                    int count = (int)item.SoLuongSp;
                    int sum = 0;
                    foreach (var j in hd)
                    {
                        if (j.MaSp == item.MaSp)
                        {
                            sum += (int)j.SoLuongBan;

                        }
                    }
                    count = count - sum;
                    HangTonKho hangTonKho = new HangTonKho();
                    hangTonKho.sanPham = item;
                    if (count >= 0)
                        hangTonKho.count = count;

                    TonKhoList.Add(hangTonKho);
                }
                else
                {
                    //sp đã bị xóa hoặc hết hàng
                }

            }
            PageNumberCurrent = 1;
            var q = TonKhoList.Skip((PageNumberCurrent - 1) * 9).Take(9);
            TonKhoFix = q.ToList();
            OnPropertyChanged("TonKhoList");
            OnPropertyChanged("TonKhoFix");
            //load sản phẩm
            // lỗi số lượng


        }
    }
}

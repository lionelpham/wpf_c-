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
    public class ManageAccViewModel : BaseViewModel
    {
        private ObservableCollection<User> _ListAccounts;
        public ObservableCollection<User> ListAccounts { get => _ListAccounts; set { _ListAccounts = value; OnPropertyChanged(); } }
        public ICommand AddCommandUser { get; set; }
        public ICommand EditCommandUser { get; set; }
        public ICommand DeleCommandUser { get; set; }
        private int _Id;
        public int Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }

        private string _Name;
        public string Name { get => _Name; set { _Name = value; OnPropertyChanged(); } }

        private string _username;
        public string username { get => _username; set { _username = value; OnPropertyChanged(); } }

        private string _pass;
        public string pass { get => _pass; set { _pass = value; OnPropertyChanged(); } }

        private int _IdRole;
        public int IdRole { get => _IdRole; set { _IdRole = value; OnPropertyChanged(); } }

        private User _SelectedItemUser;

        public User SelectedItemUser
        {
            get => _SelectedItemUser;
            set
            {
                _SelectedItemUser = value;
                OnPropertyChanged();
                if (SelectedItemUser != null)
                {
                    Id = SelectedItemUser.Id;
                    Name = SelectedItemUser.Name;
                    username = SelectedItemUser.username;
                    pass = SelectedItemUser.pass;
                    IdRole = SelectedItemUser.IdRole;
                  
                }
                //OnPropertyChanged();
            }
        }
        

        public ManageAccViewModel()
        {
            ListAccounts = new ObservableCollection<User>(DataProvider.Instace.Data.Users);
            EditCommandUser = new RelayCommand<object>((p) =>
            {
                if (SelectedItemUser.Id!=Id || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pass) || IdRole == 0)
                    return false;

                return true;
            },(p) =>
           {


               var s = DataProvider.Instace.Data.Users.Where(z => z.Id == SelectedItemUser.Id).SingleOrDefault();

               s.Id = Id;
               s.Name = Name;
               s.username = username;
               s.pass = pass;
               s.IdRole = IdRole;
               // var sp1 = new SanPham() { TenSp = TenSp, MaSp = MaSp, NgayNhap = NgayNhap, SoLuongSp = SoLuongSp, MaLoai = MaLoai, GiaSp = GiaSp, HoaDons = null };


               //DataProvider.Instace.Data.SanPhams.Add(sp1);
               DataProvider.Instace.Data.SaveChanges();

               
               OnPropertyChanged("ListAccounts");

               //TonKhoList.Add(updateTonKho);
               MessageBox.Show("Chỉnh sửa thành công");
               Id = 0;
               Name = "";
               username = "";
               pass = "";
               IdRole = 0;
               
           });
            AddCommandUser = new RelayCommand<object>(p =>
            {
                var q = DataProvider.Instace.Data.Users.Where(s=>s.Id == Id);
                var c = DataProvider.Instace.Data.UserRoles.Where(t => t.IdRole == IdRole);
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pass) ||c.Count()<=0 || q.Count() >0)
                    return false;
                
                return true;
            },p => 
            {
                User us = new User() { Id = Id, Name = Name, username = username, pass = pass, IdRole = IdRole };
                DataProvider.Instace.Data.Users.Add(us);
                DataProvider.Instace.Data.SaveChanges();
                ListAccounts.Add(us);
                MessageBox.Show("Thêm thành công");
                Id = 0;
                Name = "";
                username = "";
                pass = "";
                IdRole = 0;
            });
            DeleCommandUser = new RelayCommand<object>(p => false, p => { });
        }
    }
}

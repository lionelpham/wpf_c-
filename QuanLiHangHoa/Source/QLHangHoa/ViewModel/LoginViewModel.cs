using QLHangHoa.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QLHangHoa.ViewModel
{
    class LoginViewModel : BaseViewModel
    {
        //xử lí chính
        public bool isLogined { get; set; }
        public bool isAdmin { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand ExitLoginCommand { get; set; }
        public ICommand PasswordChangedCommand {get;set;}
        private string _username="";
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        private string _password = "";
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        private string errLogin = "Sai tên tài khoản hoặc mật khẩu";
        public string ErrLogin { get => errLogin; set { value = errLogin; OnPropertyChanged(); } }
        public LoginViewModel()
        {
            //only one
            isLogined = false;
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>{Login(p);});
            ExitLoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                isLogined = false;
                if (p == null)
                    return;
                p.Close();
            });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
        }
        void Login(Window p)
        {
            if (p == null)
                return;
            
            var counter = DataProvider.Instace.Data.Users.Where(x=>x.username == Username && x.pass == Password);
            
            if (counter.Count() == 1 )
            {
                if (counter.Sum(q=>q.IdRole) == 1)
                {
                    isAdmin = true;
                    
                }
                if (counter.Sum(q => q.IdRole) == 2)
                {
                    isAdmin = false;

                }
                isLogined = true;
                p.Close();
                ErrLogin = "";
            }
            else
            {
                isLogined = false;
                ErrLogin = errLogin;
                OnPropertyChanged();
                MessageBox.Show("Sai tài khoản hoặc mật khẩu", "Lỗi đăng nhập");
            }
           
        }

    }
}

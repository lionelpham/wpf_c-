using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLHangHoa.Model
{
    class DataProvider
    {
        private static DataProvider _instance;
        public static DataProvider Instace
        {
            get
            {
                if (_instance == null)
                    _instance = new DataProvider();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        public QuanLiHangHoaEntities Data { get; set; }
        private DataProvider()
        {
            Data = new QuanLiHangHoaEntities();
        }
    }
}

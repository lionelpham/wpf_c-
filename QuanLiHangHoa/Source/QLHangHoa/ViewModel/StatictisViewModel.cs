using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using QLHangHoa.Model;

namespace QLHangHoa.ViewModel
{
    public class StatictisViewModel : BaseViewModel
    {

        private string _day = "Ngày";
        public string day { get => _day; set { _day = value; OnPropertyChanged(); } }


        private string _month = "Tháng";
        public string month { get => _month; set { _month = value; OnPropertyChanged(); } }

        private string _year = "Năm";
        public string year { get => _year; set { _year = value; OnPropertyChanged(); } }

        private string _SelectedFilter;
        public string SelectedFilter
        {
            get => _SelectedFilter;
            set
            {
                _SelectedFilter = value;
                OnPropertyChanged();
                if (SelectedFilter != null)
                {
                    switch (SelectedFilter)
                    {
                        case "Ngày":
                            // OnPropertyChanged("DayBinding");
                            //MessageBox.Show(DayBinding.ToString());
                            loadDataDay();
                            //filter day
                            break;
                        case "Tháng":
                            loadDataMonth();
                            //filter month
                            break;
                        case "Năm":
                            loadDataYear();
                            //filter year
                            break;
                    }
                }
                //OnPropertyChanged();
            }
        }

        private int _index = 0;
        public int index { get => _index; set { _index = value; OnPropertyChanged(); } }


        private DateTime _DayBinding = DateTime.Now;
        public DateTime DayBinding { get => _DayBinding; set { _DayBinding = value; OnPropertyChanged(); } }

        private DateTime _fromDay;
        public DateTime fromDay { get => _fromDay; set { _fromDay = value; OnPropertyChanged(); } }

        private DateTime _toDay;
        public DateTime toDay { get => _toDay; set { _toDay = value; OnPropertyChanged(); } }

        private int? _soLuongBan;
        public int? soLuongBan { get => _soLuongBan; set { _soLuongBan = value; OnPropertyChanged(); } }

        private bool _HintCol;
        public bool HintCol { get => _HintCol; set { _HintCol = value; OnPropertyChanged(); } }

        private bool _HintPie;
        public bool HintPie { get => _HintPie; set { _HintPie = value; OnPropertyChanged(); } }

        private bool _HintLine = false;
        public bool HintLine { get => _HintLine; set { _HintLine = value; OnPropertyChanged(); } }

        private DateTime? _ngayBan;
        public DateTime? ngayBan { get => _ngayBan; set { _ngayBan = value; OnPropertyChanged(); } }

        private string _MaLoai;
        public string MaLoai { get => _MaLoai; set { _MaLoai = value; OnPropertyChanged(); } }

        private string _TenLoai;
        public string TenLoai { get => _TenLoai; set { _TenLoai = value; OnPropertyChanged(); } }

        private int? _tongTien;
        public int? tongTien { get => _tongTien; set { _tongTien = value; OnPropertyChanged(); } }


        private List<string> _ListFilter;
        public List<string> ListFilter { get => _ListFilter; set { _ListFilter = value; OnPropertyChanged(); } }

        private SeriesCollection _SeriesCollectionCol;
        public SeriesCollection SeriesCollectionCol { get => _SeriesCollectionCol; set { _SeriesCollectionCol = value; OnPropertyChanged(); } }

        private SeriesCollection _SeriesCollectionPie;
        public SeriesCollection SeriesCollectionPie { get => _SeriesCollectionPie; set { _SeriesCollectionPie = value; OnPropertyChanged(); } }

        private SeriesCollection _SeriesCollectionLine;
        public SeriesCollection SeriesCollectionLine { get => _SeriesCollectionLine; set { _SeriesCollectionLine = value; OnPropertyChanged(); } }

        public Func<double, string> Formatter { get; set; }
        private ObservableCollection<ReportSale> _ReportList;
        public ObservableCollection<ReportSale> ReportList { get => _ReportList; set { _ReportList = value; OnPropertyChanged(); } }

        public ICommand ViewCommand { get; set; }
        public ICommand ChangeCommand { get; set; }
        public StatictisViewModel()
        {
            SeriesCollectionCol = new SeriesCollection();
            SeriesCollectionPie = new SeriesCollection();
            SeriesCollectionLine = new SeriesCollection();
            ListFilter = new List<string>();
            ListFilter.Add(day);
            ListFilter.Add(month);
            ListFilter.Add(year);

            ReportList = new ObservableCollection<ReportSale>();
            loadProductSale();

            ViewCommand = new RelayCommand<object>(
            (p) =>
            {
                if (fromDay == null && toDay == null)
                    return false;
                return true;
            },
            (p) =>
            {

                loadDataChartFromInputUser();
                OnPropertyChanged();
            });

            ChangeCommand = new RelayCommand<object>(
            (p) =>
            {
                if (DayBinding == null)
                    return false;
                return true;
            },
            (p) =>
            {
                switch (SelectedFilter)
                {
                    case "Ngày":
                        // OnPropertyChanged("DayBinding");
                        //MessageBox.Show(DayBinding.ToString());
                        loadDataDay();
                        //filter day
                        break;
                    case "Tháng":
                        loadDataMonth();
                        //filter month
                        break;
                    case "Năm":
                        loadDataYear();
                        //filter year
                        break;
                }

                //loadChart();
                OnPropertyChanged();
            });
        }

        private void loadDataChartFromInputUser()
        {
            SeriesCollectionCol.Clear();
            SeriesCollectionPie.Clear();
            SeriesCollectionLine.Clear();

            foreach (var i in DataProvider.Instace.Data.LoaiSanPhams)
            {
                var query = from r in ReportList
                            where  DateTime.Compare(r.ngayBan.Value.Date, fromDay.Date) >=0 && DateTime.Compare(r.ngayBan.Value.Date, toDay.Date) <= 0
                            && r.TenLoai == i.TenLoai
                            select r;

                var tongSoLuong = query.Sum(r => r.soLuongBan);
                var tongTien = query.Sum(r => r.tongTien);
                var col = new ColumnSeries() { Title = i.TenLoai, Values = new ChartValues<double> { (double)tongTien.Value } };
                var pie = new PieSeries()
                {
                    Title = i.TenLoai,
                    Values = new ChartValues<int> { tongSoLuong.Value },
                    DataLabels = true,
                    LabelPoint = chartPoint =>
                 string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation)
                };

                SeriesCollectionCol.Add(col);
                SeriesCollectionPie.Add(pie);
                //TitleColX = "Loại Sản Phẩm";
                Formatter = value => value.ToString("#,##0 VND");
            }
        }

        private void loadProductSale()
        {
            //Tên loại sản phẩm
            //Số lượng hóa đơn cho mã sản phẩm
            //Tiền

            var s = from v in DataProvider.Instace.Data.HoaDons
                    join t in DataProvider.Instace.Data.SanPhams
                    on v.MaSp equals t.MaSp
                    join k in DataProvider.Instace.Data.LoaiSanPhams
                    on t.MaLoai equals k.MaLoai

                    select new
                    {
                        TenLoaiSp = k.TenLoai,
                        MaLoaiSp = k.MaLoai,
                        NgayBanSp = v.NgayBan,
                        TongTien = t.GiaSp * v.SoLuongBan,
                        SoLuongSpBan = v.SoLuongBan,

                    };

            foreach (var k in s)
            {
                MaLoai = k.MaLoaiSp;
                TenLoai = k.TenLoaiSp;
                ngayBan = k.NgayBanSp;
                tongTien = k.TongTien;
                soLuongBan = k.SoLuongSpBan;
                ReportSale sale = new ReportSale() { MaLoai = MaLoai, TenLoai = TenLoai, ngayBan = ngayBan, tongTien = tongTien, soLuongBan = soLuongBan };
                ReportList.Add(sale);
            }


            //ReportSale sale = new ReportSale() { MaLoai = MaLoai, TenLoai = TenLoai, ngayBan = ngayBan, tongTien = tongTien, tongSoLuongBan = tongSoLuongBan };
            //



        }
        void loadDataDay()
        {
            SeriesCollectionCol.Clear();
            SeriesCollectionPie.Clear();
            SeriesCollectionLine.Clear();

            foreach (var i in DataProvider.Instace.Data.LoaiSanPhams)
            {
                var query = from r in ReportList
                            where r.ngayBan.Value.Day == DayBinding.Day && r.ngayBan.Value.Month == DayBinding.Month && r.ngayBan.Value.Year == DayBinding.Year
                            && r.TenLoai == i.TenLoai
                            select r;

                var tongSoLuong = query.Sum(r => r.soLuongBan);
                var tongTien = query.Sum(r => r.tongTien);
                var col = new ColumnSeries() { Title = i.TenLoai, Values = new ChartValues<double> { (double)tongTien.Value } };
                var pie = new PieSeries()
                {
                    Title = i.TenLoai,
                    Values = new ChartValues<int> { tongSoLuong.Value },
                    DataLabels = true,
                    LabelPoint = chartPoint =>
                 string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation)
                };

                SeriesCollectionCol.Add(col);
                SeriesCollectionPie.Add(pie);
                //TitleColX = "Loại Sản Phẩm";
                Formatter = value => value.ToString("#,##0 VND");
            }
        }

        void loadDataYear()
        {
            SeriesCollectionCol.Clear();
            SeriesCollectionPie.Clear();
            SeriesCollectionLine.Clear();
            
            foreach (var i in DataProvider.Instace.Data.LoaiSanPhams)
            {
                var query = from r in ReportList
                            where r.ngayBan.Value.Year == DayBinding.Year
                            && r.TenLoai == i.TenLoai
                            select r;
                var tongSoLuong = query.Sum(r => r.soLuongBan);
                var tongTien = query.Sum(r => r.tongTien);
                var col = new ColumnSeries() { Title = i.TenLoai, Values = new ChartValues<double> { (double)tongTien.Value } };
                var pie = new PieSeries()
                {
                    Title = i.TenLoai,
                    Values = new ChartValues<int> { tongSoLuong.Value },
                    DataLabels = true,
                    LabelPoint = chartPoint =>
                 string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation)
                };

                SeriesCollectionCol.Add(col);
                SeriesCollectionPie.Add(pie);
            }
            List<double> values = new List<double>();
            List<string> str = new List<string>();
            
            for (int i = 1; i <= 12; i++)
            {
                var temp = from r in ReportList
                           where r.ngayBan.Value.Month == i && r.ngayBan.Value.Year == DayBinding.Year

                           select r;
                double month;
                try
                {
                    month = temp.Sum(r => r.tongTien).Value;

                }
                catch (Exception)
                {

                    month = 0;
                }

                values.Add(month);
                str.Add("Tháng " + i.ToString());

            }
            var line = new LineSeries() { Title = "Tổng thu nhập", Values = new ChartValues<double>(values) };
            SeriesCollectionLine.Add(line);

        }
        void loadDataMonth()
        {
            SeriesCollectionCol.Clear();
            SeriesCollectionPie.Clear();
            SeriesCollectionLine.Clear();


            foreach (var i in DataProvider.Instace.Data.LoaiSanPhams)
            {
                var query = from r in ReportList
                            where r.ngayBan.Value.Month == DayBinding.Month && r.ngayBan.Value.Year == DayBinding.Year
                            && r.TenLoai == i.TenLoai
                            select r;
                var tongSoLuong = query.Sum(r => r.soLuongBan);
                var tongTien = query.Sum(r => r.tongTien);
                var col = new ColumnSeries() { Title = i.TenLoai, Values = new ChartValues<double> { (double)tongTien.Value } };
                var pie = new PieSeries()
                {
                    Title = i.TenLoai,
                    Values = new ChartValues<int> { tongSoLuong.Value },
                    DataLabels = true,
                    LabelPoint = chartPoint =>
                 string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation)
                };

                SeriesCollectionCol.Add(col);
                SeriesCollectionPie.Add(pie);


            }
            //Line chart
            List<double> values = new List<double>();
            List<string> str = new List<string>();
            int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            for (int i = 1; i <= days; i++)
            {
                var temp = from r in ReportList
                           where r.ngayBan.Value.Month == DayBinding.Month && r.ngayBan.Value.Year == DayBinding.Year
                           && r.ngayBan.Value.Day == i
                           select r;
                double day;
                try
                {
                    day = temp.Sum(r => r.tongTien).Value;

                }
                catch (Exception)
                {

                    day = 0;
                }

                values.Add(day);
                str.Add("Ngày " + i.ToString());

            }


            SeriesCollectionLine.Clear();
            var line = new LineSeries() { Title = "Tổng tiền", Values = new ChartValues<double>(values) };
            SeriesCollectionLine.Add(line);

        }

    }
}


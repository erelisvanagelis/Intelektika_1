using Caliburn.Micro;
using Intelektika_1.Models;
using Intelektika_1.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Intelektika_1.ViewModels
{
    class MainViewModel : Screen
    {
        private BindableCollection<DataSetModel> _dataSetModels;
        private BindableCollection<DataSetDistanceModel> _dataSetDistanceModels;
        private BindableCollection<string> _sports;
        private BindableCollection<string> _positions;

        private string _height = "";
        private string _weight = "";
        private string _sport = "";
        private string _position = "";

        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private Stopwatch stopwatch = new Stopwatch();
        private string _time = "";

        public BindableCollection<DataSetModel> DataSetModels
        {
            get => _dataSetModels; set
            {
                _dataSetModels = value;
                NotifyOfPropertyChange(() => DataSetModels);
            }
        }
        public BindableCollection<string> Sports
        {
            get => _sports; set
            {
                _sports = value;
                NotifyOfPropertyChange(() => Sports);
            }
        }
        public BindableCollection<DataSetDistanceModel> DataSetDistanceModels
        {
            get => _dataSetDistanceModels; set
            {
                _dataSetDistanceModels = value;
                NotifyOfPropertyChange(() => DataSetDistanceModels);
            }
        }
        public BindableCollection<string> Positions
        {
            get => _positions; set
            {
                _positions = value;
                NotifyOfPropertyChange(() => Positions);
            }
        }
        public string Height
        {
            get => _height; set
            {
                _height = value;
                NotifyOfPropertyChange(() => Height);
            }
        }
        public string Weight
        {
            get => _weight; set
            {
                _weight = value;
                NotifyOfPropertyChange(() => Weight);
            }
        }
        public string Sport
        {
            get => _sport; set
            {
                _sport = value;
                NotifyOfPropertyChange(() => Sport);
            }
        }
        public string Position
        {
            get => _position; set
            {
                _position = value;
                NotifyOfPropertyChange(() => Position);
            }
        }
        public string Time
        {
            get => _time; set
            {
                _time = value;
                NotifyOfPropertyChange(() => Time);
            }
        }

        public MainViewModel()
        {
            DataSetModels = new BindableCollection<DataSetModel>();
            DataSetDistanceModels = new BindableCollection<DataSetDistanceModel>();
            Sports = new BindableCollection<string>();
            Positions = new BindableCollection<string>();

            dispatcherTimer.Tick += new EventHandler(dt_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Start();

            ResetWindow();
        }

        public void ReadJsonFile(string path)
        {
            string jsonData = File.ReadAllText(path);
            DataSetModel[] dataSets = DataSetModel.FromJson(jsonData);
            foreach (DataSetModel dsm in dataSets)
            {
                DataAcess.SaveDataSet(dsm);
            }
            ResetWindow();
        }

        public bool CanAddData(string height, string weight, string sport, string position) => !AreStringsNullOrEmpty(height, weight, sport, position);
        public void AddData(string height, string weight, string sport, string position)
        {
            Trace.WriteLine($"AddData: {Height}, {Weight}, {Sport}, {Position},");
            var dsm = new DataSetModel()
            {
                Height = Convert.ToInt32(height),
                Weight = Convert.ToInt32(weight),
                Sport = sport,
                Position = position,
            };

            DataAcess.SaveDataSet(dsm);
            ResetWindow();
        }

        public void ClearInputData()
        {
            Height = "";
            Weight = "";
            Sport = "";
            Position = "";
        }

        public void ResetWindow()
        {
            ClearInputData();
            DataSetModels.Clear();
            DataSetDistanceModels.Clear();
            Sports.Clear();
            Positions.Clear();

            DataSetModels.AddRange(DataAcess.LoadDataSets());
            Sports.AddRange(DataAcess.LoadSports());
            Sport = Sports.FirstOrDefault();
        }

        public void ClearDataSets()
        {
            DataAcess.DeleteDBEntries();
            ResetWindow();
        }

        public bool CanDefinePosition(string height, string weight, string sport) => !AreStringsNullOrEmpty(height, weight, sport);
        public async void DefinePosition(string height, string weight, string sport)
        {
            stopwatch.Reset();
            stopwatch.Start();
            var dsm = new DataSetModel
            {
                Weight = Convert.ToInt32(weight),
                Height = Convert.ToInt32(height),
                Sport = sport
            };

            //await Task.Delay(1000);
            var result = await Task.FromResult(DataClasification.GetClosestSets(DataSetModels.ToList(), dsm));
            var ordered = result.OrderBy(x => x.Distance).ToList();
            DataSetDistanceModels.Clear();
            Positions.Clear();
            DataSetDistanceModels.AddRange(ordered);
            Position = DataSetDistanceModels.First().Position;
            Positions.AddRange(ordered.ExtractDistancePosition());

            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            Time = String.Format("{0:00}:{1:00}", ts.Seconds, ts.Milliseconds);
        }

        void dt_Tick(object sender, EventArgs e)
        {
            if (stopwatch.IsRunning)
            {
                TimeSpan ts = stopwatch.Elapsed;
                Time = String.Format("{0:00}:{1:00}", ts.Seconds, ts.Milliseconds);
            }
        }

        public static bool AreStringsNullOrEmpty(params string[] strings)
        {
            foreach (string s in strings)
            {
                if (String.IsNullOrWhiteSpace(s))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

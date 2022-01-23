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


namespace Intelektika_1.ViewModels
{
    class MainViewModel : Screen
    {
        private BindableCollection<DataSetModel> _dataSetModels;
        private BindableCollection<DataSetDistanceModel> _dataSetDistanceModels;
        private BindableCollection<string> _sports;
        private BindableCollection<string> _positions;
        private string _selectedSport;
        private string _selectedPosition;
        private string _addHeight = "";
        private string _addWeight = "";
        private string _addSport = "";
        private string _addPosition = "";
        private string _defineHeight = "";
        private string _defineWeight = "";
        private string _time = "";

        private string _test = "";
        private BindableCollection<string> _tests = new();



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

        public string AddHeight
        {
            get => _addHeight; set
            {
                _addHeight = value;
                NotifyOfPropertyChange(() => AddHeight);
            }
        }
        public string AddWeight
        {
            get => _addWeight; set
            {
                _addWeight = value;
                NotifyOfPropertyChange(() => AddWeight);
            }
        }
        public string AddSport
        {
            get => _addSport; set
            {
                _addSport = value;
                NotifyOfPropertyChange(() => AddSport);
            }
        }
        public string AddPosition
        {
            get => _addPosition; set
            {
                _addPosition = value;
                NotifyOfPropertyChange(() => AddPosition);
            }
        }

        public string SelectedSport
        {
            get => _selectedSport; set
            {
                _selectedSport = value;
                NotifyOfPropertyChange(() => SelectedSport);
            }
        }

        public string DefineHeight
        {
            get => _defineHeight; set
            {
                _defineHeight = value;
                NotifyOfPropertyChange(() => DefineHeight);
            }
        }
        public string DefineWeight
        {
            get => _defineWeight; set
            {
                _defineWeight = value;
                NotifyOfPropertyChange(() => DefineWeight);
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

        public string SelectedPosition
        {
            get => _selectedPosition; set
            {
                _selectedPosition = value;
                NotifyOfPropertyChange(() => SelectedPosition);
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

        public string Time
        {
            get => _time; set
            {
                _time = value;
                NotifyOfPropertyChange(() => Time);
            }
        }

        public string Test
        {
            get => _test; set
            {
                _test = value;
                NotifyOfPropertyChange(() => Test);
            }
        }
        public BindableCollection<string> Tests
        {
            get => _tests; set
            {
                _tests = value;
                NotifyOfPropertyChange(() => Tests);
            }
        }

        public MainViewModel()
        {
            Tests.Add("test0");
            Tests.Add("test1");
            Tests.Add("test2");
            Tests.Add("test3");
            Tests.Add("test4");
            DataSetModels = new BindableCollection<DataSetModel>();
            DataSetDistanceModels = new BindableCollection<DataSetDistanceModel>();
            Sports = new BindableCollection<string>();
            Positions = new BindableCollection<string>();
            GetDataFromDB();
        }

        public void TraceTest()
        {
            Trace.WriteLine(Test);
        }

        public void GetDataFromDB()
        {
            DataSetModels.Clear();
            DataSetDistanceModels.Clear();
            Sports.Clear();
            Positions.Clear();

            var dataSets = DataAcess.LoadDataSets();
            var sports = DataAcess.LoadSports();
            DataSetModels.AddRange(dataSets);
            Sports.AddRange(sports);
            SelectedSport = Sports.FirstOrDefault();
        }

        public void ReadJsonFile(string path)
        {
            string jsonData = File.ReadAllText(path);
            DataSetModel[] sets = DataSetModel.FromJson(jsonData);

            AddDataSets(sets);
            GetDataFromDB();
        }

        public static void AddDataSets(DataSetModel[] dataSets)
        {
            foreach (DataSetModel dsm in dataSets)
            {
                AddDataSet(dsm);
            }
        }

        public static bool CanAddData(string addHeight, string addWeight, string addSport, string addPosition)
        {
            Trace.WriteLine($"AddData: {addHeight} {addWeight} {addSport} {addPosition}");
            if (String.IsNullOrWhiteSpace(addHeight) || String.IsNullOrWhiteSpace(addWeight) || String.IsNullOrWhiteSpace(addSport) || String.IsNullOrWhiteSpace(addPosition))
            {
                return false;
            }
            return true;
        }

        public void AddData(string addHeight, string addWeight, string addSport, string addPosition)
        {
            Trace.WriteLine($"AddData: {AddHeight} {AddWeight} {AddSport} {AddPosition}");
            DataSetModel dataSet = new();
            dataSet.Height = Convert.ToInt32(addHeight);
            dataSet.Weight = Convert.ToInt32(addWeight);
            dataSet.Sport = addSport;
            dataSet.Position = addPosition;
            AddDataSet(dataSet);
            GetDataFromDB();
            ClearAddData();
        }

        public void ClearAddData()
        {
            AddHeight = "";
            AddWeight = "";
            AddSport = "";
            AddPosition = "";
        }

        public static void AddDataSet(DataSetModel dataSet)
        {
            DataAcess.SaveDataSet(dataSet);
        }

        public void ClearDataSets()
        {
            DataAcess.DeleteDBEntries();
            GetDataFromDB();
        }

        public bool CanDefinePosition(string defineHeight, string defineWeight, string selectedSport)
        {
            Trace.WriteLine($"CanDefinePosition: {defineHeight}, {defineWeight}, {selectedSport}");
            if (String.IsNullOrWhiteSpace(defineHeight) || String.IsNullOrWhiteSpace(defineWeight) || String.IsNullOrWhiteSpace(SelectedSport))
            {
                return false;
            }
            return true;

        }

        public async void DefinePosition(string defineHeight, string defineWeight, string selectedSport)
        {
            Trace.WriteLine($"DefinePosition: {SelectedSport}");
            var dsm = new DataSetModel
            {
                Weight = Convert.ToInt32(defineWeight),
                Height = Convert.ToInt32(defineHeight),
                Sport = SelectedSport
            };
            Stopwatch stopWatch = new();
            stopWatch.Start();
            var result = await Task.FromResult(DataClasification.GetClosestSets(DataSetModels.ToList(), dsm));
            Trace.WriteLine(result.Count);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Time = ts.ToString();
            var ordered = result.OrderBy(x => x.Distance).ToList();
            DataSetDistanceModels.Clear();
            Positions.Clear();
            DataSetDistanceModels.AddRange(ordered);
            SelectedPosition = DataSetDistanceModels.First().Position;
            Positions.AddRange(ordered.ExtractDistancePosition());
        }

        public void AddDefinedDataSet()
        {
            Trace.WriteLine($"AddDefinedDataSet: {SelectedSport}");

        }

        public void ClearDefined()
        {
            DefineHeight = "";
            DefineWeight = "";
            SelectedSport = Sports.First();
            SelectedPosition = null;
            Positions.Clear();
            DataSetDistanceModels.Clear();
        }
    }
}

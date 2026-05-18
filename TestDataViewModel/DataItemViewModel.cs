using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OxyPlot;
using OxyPlot.Series;
using TestDataLibrary;

namespace TestDataViewModel
{
    public class DataItemViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly DataItem model;

        public DataItem Model => model;

        public DataItemViewModel(DataItem model)
        {
            this.model = model;
            this.model.PropertyChanged += OnModelPropertyChanged;
        }

        private void OnModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName ?? "");
            if (e.PropertyName is nameof(DataItem.N)
                              or nameof(DataItem.LeftBound)
                              or nameof(DataItem.RightBound)
                              or nameof(DataItem.DList))
            {
                RaisePropertyChanged(nameof(GridRows));
                RaisePropertyChanged(nameof(PlotModel));
            }
            if (e.PropertyName == nameof(DataItem.Name))
            {
                RaisePropertyChanged(nameof(PlotModel));
            }
        }

        public string Name
        {
            get => model.Name;
            set => model.Name = value;
        }

        public DateTime Date
        {
            get => model.Date;
            set => model.Date = value;
        }

        public int N
        {
            get => model.N;
            set => model.N = value;
        }

        public double LeftBound
        {
            get => model.LeftBound;
            set => model.LeftBound = value;
        }

        public double RightBound
        {
            get => model.RightBound;
            set => model.RightBound = value;
        }

        public IReadOnlyList<double> DList => model.DList;

        public IEnumerable<RowViewModel> GridRows
        {
            get
            {
                var rows = new List<RowViewModel>();
                if (N <= 0 || RightBound <= LeftBound || DList == null) return rows;

                double h = (RightBound - LeftBound) / N;
                int count = Math.Min(N + 1, DList.Count);
                for (int i = 0; i < count; i++)
                    rows.Add(new RowViewModel { X = LeftBound + i * h, F = DList[i] });
                return rows;
            }
        }

        public PlotModel PlotModel
        {
            get
            {
                var pm = new PlotModel { Title = Name };
                var line = new LineSeries { MarkerType = MarkerType.Circle, MarkerSize = 4, Title = Name };
                foreach (var row in GridRows)
                    line.Points.Add(new DataPoint(row.X, row.F));
                pm.Series.Add(line);
                return pm;
            }
        }

        public string Error => string.Empty;

        public string this[string columnName] => columnName switch
        {
            nameof(Name) => string.IsNullOrWhiteSpace(Name) ? "Имя не может быть пустым" : string.Empty,
            nameof(LeftBound) or nameof(RightBound) =>
                LeftBound >= RightBound ? "Левая граница должна быть меньше правой" : string.Empty,
            nameof(N) => N <= 0 ? "N должно быть положительным" : string.Empty,
            nameof(Date) => IsDateInForbiddenRange(Date)
                ? "Даты с 1 по 10 мая текущего года недопустимы"
                : string.Empty,
            _ => string.Empty
        };

        private static bool IsDateInForbiddenRange(DateTime date)
        {
            int currentYear = DateTime.Now.Year;
            return date.Year == currentYear
                && date.Month == 5
                && date.Day >= 1 && date.Day <= 10;
        }
        private RelayCommand? updateElementCommand;
        public RelayCommand UpdateElementCommand =>
            updateElementCommand ??= new RelayCommand(_ => model.UpdateElement());
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace TestDataLibrary
{
    public class DataItem : INotifyPropertyChanged
    {
        private string name = string.Empty;
        private DateTime date;
        private int n;
        private double leftBound;
        private double rightBound;
        private List<double> dList = new();

        private Func<double, double> F { get; }


        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Date
        {
            get => date;
            set
            {
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged();
                }
            }
        }

        public int N
        {
            get => n;
            set
            {
                if (n != value)
                {
                    n = value;
                    OnPropertyChanged();
                    RecalculateValues();
                }
            }
        }

        public double LeftBound
        {
            get => leftBound;
            set
            {
                if (leftBound != value)
                {
                    leftBound = value;
                    OnPropertyChanged();
                    RecalculateValues();
                }
            }
        }

        public double RightBound
        {
            get => rightBound;
            set
            {
                if (rightBound != value)
                {
                    rightBound = value;
                    OnPropertyChanged();
                    RecalculateValues();
                }
            }
        }

        public List<double> DList
        {
            get => dList;
            set
            {
                if (dList != value)
                {
                    dList = value;
                    OnPropertyChanged();
                }
            }
        }

        public DataItem(string name, DateTime date, int n, (double, double) bounds, Func<double, double> f)
        {
            this.F = f;
            this.name = name;
            this.date = date;
            this.n = n;
            this.leftBound = bounds.Item1;
            this.rightBound = bounds.Item2;
            RecalculateValues();
        }

        public DataItem()
        {
            this.F = x => x * x;
            this.name = "Default item";
            this.date = DateTime.Now;
            this.n = 4;
            this.leftBound = 0.0;
            this.rightBound = 4.0;
            RecalculateValues();
        }

        public void UpdateElement()
        {
            Name = Name + " +";
            Date = DateTime.Now;
            N = N + 1;
        }

        private void RecalculateValues()
        {
            if (F == null) return;
            if (N <= 0) return;
            if (RightBound <= LeftBound) return;

            var newList = new List<double>();
            double h = (RightBound - LeftBound) / N;
            for (int i = 0; i <= N; i++)
            {
                double x = LeftBound + i * h;
                newList.Add(F(x));
            }
            DList = newList;
        }


        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Name: {Name}");
            sb.AppendLine($"Date: {Date}");
            sb.AppendLine($"N: {N}");
            sb.AppendLine($"Bounds: ({LeftBound}; {RightBound})");
            sb.Append("DList: ");
            for (int i = 0; i < DList.Count; i++)
            {
                sb.Append(DList[i].ToString("F2"));
                if (i < DList.Count - 1) sb.Append(", ");
            }
            return sb.ToString();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
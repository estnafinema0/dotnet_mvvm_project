using System;
using System.Collections.Generic;
using System.ComponentModel;
using TestDataLibrary;
using TestDataViewModel;
using OxyPlot.Series;

namespace TestDataViewModel.Tests
{
    public class DataItemViewModelTests
    {
        private static DataItemViewModel MakeVM(int n = 4, double a = 0.0, double b = 2.0,
                                                Func<double, double>? f = null, string name = "Test")
        {
            return new DataItemViewModel(
                new DataItem(name, DateTime.Now, n, (a, b), f ?? (x => x * x)));
        }

        [Fact]
        public void SettingNameOnVm_UpdatesModel()
        {
            var vm = MakeVM();
            vm.Name = "Changed";
            vm.Model.Name.Should().Be("Changed");
        }

        [Fact]
        public void ChangingModelProperty_RaisesPropertyChangedOnVm()
        {
            var vm = MakeVM();
            var raised = new List<string?>();
            vm.PropertyChanged += (_, e) => raised.Add(e.PropertyName);

            vm.Model.N = 10;

            raised.Should().Contain("N");
            raised.Should().Contain(nameof(DataItemViewModel.GridRows));
            raised.Should().Contain(nameof(DataItemViewModel.PlotModel));
        }

        [Fact]
        public void Validation_EmptyName_ReturnsError()
        {
            var vm = MakeVM(name: "");
            ((IDataErrorInfo)vm)[nameof(DataItemViewModel.Name)].Should().NotBeEmpty();
        }

        [Fact]
        public void Validation_LeftBoundGreaterThanRight_ReturnsError()
        {
            var vm = MakeVM(a: 5.0, b: 1.0, f: x => x);
            ((IDataErrorInfo)vm)[nameof(DataItemViewModel.LeftBound)].Should().NotBeEmpty();
            ((IDataErrorInfo)vm)[nameof(DataItemViewModel.RightBound)].Should().NotBeEmpty();
        }

        [Fact]
        public void Validation_ValidValues_NoErrors()
        {
            var vm = MakeVM();
            ((IDataErrorInfo)vm)[nameof(DataItemViewModel.Name)].Should().BeEmpty();
            ((IDataErrorInfo)vm)[nameof(DataItemViewModel.N)].Should().BeEmpty();
            ((IDataErrorInfo)vm)[nameof(DataItemViewModel.LeftBound)].Should().BeEmpty();
        }

        [Fact]
        public void GridRows_CountEqualsNPlusOne()
        {
            var vm = MakeVM(n: 5, a: 0, b: 1, f: x => x);
            System.Linq.Enumerable.Count(vm.GridRows).Should().Be(6);
        }

        [Fact]
        public void GridRows_FValuesMatchFunction()
        {
            var vm = MakeVM(n: 4, a: 0, b: 2, f: x => x * x);
            foreach (var row in vm.GridRows)
                row.F.Should().BeApproximately(row.X * row.X, 1e-10);
        }

        [Fact]
        public void PlotModel_ContainsLineSeriesWithCorrectPoints()
        {
            var vm = MakeVM(n: 4, a: 0, b: 2, f: x => x * x);
            var pm = vm.PlotModel;

            pm.Series.Should().HaveCount(1);
            pm.Series[0].Should().BeAssignableTo<LineSeries>();
            var line = (LineSeries)pm.Series[0];
            line.Points.Should().HaveCount(5);
            foreach (var p in line.Points)
                p.Y.Should().BeApproximately(p.X * p.X, 1e-10);
        }

        [Fact]
        public void UpdateElementCommand_IncrementsN()
        {
            var vm = MakeVM(n: 4);
            int before = vm.N;
            vm.UpdateElementCommand.Execute(null);
            vm.N.Should().Be(before + 1);
        }
    }
}
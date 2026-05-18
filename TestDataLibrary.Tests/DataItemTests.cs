using System;
using TestDataLibrary;

namespace TestDataLibrary.Tests
{
    public class DataItemTests
    {
        [Fact]
        public void Constructor_StoresAllProperties()
        {
            var date = new DateTime(2024, 1, 1);
            var item = new DataItem("Test", date, 4, (0.0, 2.0), x => x * x);

            item.Name.Should().Be("Test");
            item.Date.Should().Be(date);
            item.N.Should().Be(4);
            item.LeftBound.Should().Be(0.0);
            item.RightBound.Should().Be(2.0);
        }

        [Fact]
        public void Constructor_CalculatesDListCorrectly()
        {
            // f(x) = x^2 на [0, 2] с N=4 → узлы 0, 0.5, 1, 1.5, 2
            var item = new DataItem("Sq", DateTime.Now, 4, (0.0, 2.0), x => x * x);

            item.DList.Should().HaveCount(5);
            item.DList[0].Should().BeApproximately(0.0, 1e-10);
            item.DList[1].Should().BeApproximately(0.25, 1e-10);
            item.DList[2].Should().BeApproximately(1.0, 1e-10);
            item.DList[3].Should().BeApproximately(2.25, 1e-10);
            item.DList[4].Should().BeApproximately(4.0, 1e-10);
        }

        [Fact]
        public void ChangingN_RecalculatesDList()
        {
            var item = new DataItem("Sq", DateTime.Now, 2, (0.0, 2.0), x => x);
            item.DList.Should().HaveCount(3);

            item.N = 4;

            item.DList.Should().HaveCount(5);
            item.DList[0].Should().BeApproximately(0.0, 1e-10);
            item.DList[4].Should().BeApproximately(2.0, 1e-10);
        }

        [Fact]
        public void ChangingBounds_RecalculatesDList()
        {
            var item = new DataItem("F", DateTime.Now, 2, (0.0, 1.0), x => x);
            item.DList[2].Should().BeApproximately(1.0, 1e-10);

            item.RightBound = 2.0;

            item.DList[2].Should().BeApproximately(2.0, 1e-10);
        }

        [Fact]
        public void UpdateElement_IncrementsNAndAppendsToName()
        {
            var item = new DataItem("X", DateTime.Now, 4, (0.0, 1.0), x => x);
            var initialN = item.N;
            var initialName = item.Name;

            item.UpdateElement();

            item.N.Should().Be(initialN + 1);
            item.Name.Should().Be(initialName + " +");
        }

        [Fact]
        public void SettingProperty_RaisesPropertyChanged()
        {
            var item = new DataItem("X", DateTime.Now, 4, (0.0, 1.0), x => x);
            var raised = new System.Collections.Generic.List<string?>();
            item.PropertyChanged += (_, e) => raised.Add(e.PropertyName);

            item.Name = "New";
            item.N = 8;

            raised.Should().Contain("Name");
            raised.Should().Contain("N");
        }

        [Fact]
        public void ToString_ContainsAllFieldNames()
        {
            var item = new DataItem("Test", new DateTime(2024, 1, 1), 4, (0.0, 2.0), x => x);
            var s = item.ToString();

            s.Should().Contain("Name");
            s.Should().Contain("Date");
            s.Should().Contain("N");
            s.Should().Contain("Bounds");
            s.Should().Contain("DList");
        }
    }
}
using System;
using System.Collections.ObjectModel;

namespace TestDataLibrary
{
    public class DataCollection : ObservableCollection<DataItem>
    {
        public DataCollection()
        {
            Add(new DataItem("Sin", DateTime.Now, 4, (0.0, 3.14), Math.Sin));
            Add(new DataItem("Tan", DateTime.Now.AddDays(-1), 5, (0.0, 3.14), Math.Tan));
            Add(new DataItem("Cube", DateTime.Now.AddDays(-2), 6, (-2.0, 2.0), x => x * x * x));
        }

        public void UpdateCollection()
        {
            if (Count > 0)
            {
                this[0].Name = "Updated item";
                this[0].Date = DateTime.Now;
                this[0].N = 8;
                this[0].LeftBound = -1.0;
                this[0].RightBound = 1.0;
            }

            Add(new DataItem("New item seq", DateTime.Now, 5, (1.0, 6.0), x => 2 * x + 1));
        }


        public override string ToString()
        {
            string result = "";

            foreach (DataItem item in this)
            {
                result += item.ToString() + Environment.NewLine + "--------------------" + Environment.NewLine;
            }

            return result;
        }
    }
}
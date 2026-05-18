using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace TestDataLibrary
{
    public enum Criteria
    {
        MinLength, 
        MaxLength, 
        MiddleLength
    }
    public class TestData
    {
        public List<string> StringList { get; set; }
        public DateTime TestDate { get; set; }
        public bool IsSaved { get; set; }

        public Criteria Criteria { get; set; }
        public TestData()
        {
            StringList = new List<string>();
            TestDate = DateTime.Now;
            IsSaved = false;
            Criteria = Criteria.MinLength;
        }

        public TestData(int count)
        {
            StringList = new List<string>();
            TestDate = DateTime.Now;
            IsSaved = false;
            Criteria = Criteria.MinLength;

            for (int i = 1; i <= count; i++)
            {
                StringList.Add($"Элемент {new string('*', i)}");
            }
        }

        public string Select(Criteria criteria)
        {
            if (StringList == null || StringList.Count == 0)
            {
                return null;
            }

            string minString = StringList[0];
            string maxString = StringList[0];

            foreach (string str in StringList)
            {
                if (str.Length > maxString.Length)
                {
                    maxString = str;
                }
                if (str.Length < minString.Length)
                {
                    minString = str;
                }
            }
        
            if (criteria == Criteria.MinLength)
            {
                return minString;
            }

            if (criteria == Criteria.MaxLength)
            {
                return maxString;
            }

            foreach (string str in StringList)
            {
                if (str.Length > minString.Length && str.Length < maxString.Length)
                {
                    return str;
                }
            }

            return null;
        }

        public void SaveData(string filename)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename, false, Encoding.UTF8))
                {
                    writer.WriteLine(TestDate.ToString("O"));
                    writer.WriteLine(Criteria);

                    foreach (string item in StringList)
                    {
                        writer.WriteLine(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void LoadData(string filename, ref TestData tdata)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filename, Encoding.UTF8))
                {
                    string dateLine = reader.ReadLine();
                    string criteriaLine = reader.ReadLine();

                    if (string.IsNullOrWhiteSpace(dateLine))
                    {
                        throw new Exception("Файл пустой или не содержит дату.");
                    }

                    if (string.IsNullOrWhiteSpace(criteriaLine))
                    {
                        throw new Exception("Файл не содержит значение критерия.");
                    }

                    TestData tempData = new TestData();
                    tempData.TestDate = DateTime.Parse(dateLine);
                    tempData.Criteria = Enum.Parse<Criteria>(criteriaLine);
                    tempData.StringList.Clear();
                    

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        tempData.StringList.Add(line);
                    }

                    tempData.IsSaved = true;
                    tdata = tempData;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override string ToString()
        {
            return $"Дата: {TestDate.ToShortDateString()}, количество элементов в списке: {StringList.Count}, IsSaved: {IsSaved}, Criteria: {Criteria}";
        }
    }
}
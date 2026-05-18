namespace TestDataViewModel
{
    public class RowViewModel
    {
        public double X { get; set; }
        public double F { get; set; }

        public override string ToString() => $"x = {X:F4};   f(x) = {F:F4}";
    }
}
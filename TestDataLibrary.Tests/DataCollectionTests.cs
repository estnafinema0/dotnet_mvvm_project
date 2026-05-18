using TestDataLibrary;

namespace TestDataLibrary.Tests
{
    public class DataCollectionTests
    {
        [Fact]
        public void Constructor_AddsThreeInitialItems()
        {
            var c = new DataCollection();
            c.Should().HaveCount(3);
            c[0].Name.Should().Be("Sin");
            c[1].Name.Should().Be("Tan");
            c[2].Name.Should().Be("Cube");
        }

        [Fact]
        public void UpdateCollection_ModifiesFirstItemAndAddsNew()
        {
            var c = new DataCollection();

            c.UpdateCollection();

            c.Should().HaveCount(4);
            c[0].Name.Should().Be("Updated item");
            c[0].N.Should().Be(8);
            c[3].Name.Should().Be("New item seq");
        }

        [Fact]
        public void ToString_ContainsAllItems()
        {
            var c = new DataCollection();
            var s = c.ToString();

            s.Should().Contain("Sin");
            s.Should().Contain("Tan");
            s.Should().Contain("Cube");
        }
    }
}
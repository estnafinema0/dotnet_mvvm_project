using Moq;
using TestDataLibrary;
using TestDataViewModel;

namespace TestDataViewModel.Tests
{
    public class MainViewModelTests
    {
        [Fact]
        public void Constructor_PopulatesItemsFromCollection()
        {
            var ui = new Mock<IUIServices>();
            var mvm = new MainViewModel(ui.Object);

            mvm.Items.Should().HaveCount(3);
            mvm.Items[0].Name.Should().Be("Sin");
        }

        [Fact]
        public void Constructor_SelectsFirstItem()
        {
            var ui = new Mock<IUIServices>();
            var mvm = new MainViewModel(ui.Object);

            mvm.SelectedItem.Should().NotBeNull();
            mvm.SelectedItem!.Name.Should().Be("Sin");
        }

        [Fact]
        public void UpdateCommand_AddsViewModelForNewItem()
        {
            var ui = new Mock<IUIServices>();
            var mvm = new MainViewModel(ui.Object);
            int before = mvm.Items.Count;

            mvm.UpdateCommand.Execute(null);

            mvm.Items.Should().HaveCount(before + 1);
            mvm.Items[^1].Name.Should().Be("New item seq");
        }

        [Fact]
        public void UpdateCommand_ChangesFirstItemProperties()
        {
            var ui = new Mock<IUIServices>();
            var mvm = new MainViewModel(ui.Object);

            mvm.UpdateCommand.Execute(null);

            mvm.Items[0].Name.Should().Be("Updated item");
            mvm.Items[0].N.Should().Be(8);
        }

        [Fact]
        public void OutputCommand_CallsShowMessage()
        {
            var ui = new Mock<IUIServices>();
            var mvm = new MainViewModel(ui.Object);

            mvm.OutputCommand.Execute(null);

            ui.Verify(s => s.ShowMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void OutputCommand_PassesCollectionToString()
        {
            var ui = new Mock<IUIServices>();
            string? captured = null;
            ui.Setup(s => s.ShowMessage(It.IsAny<string>(), It.IsAny<string>()))
              .Callback<string, string>((m, _) => captured = m);

            var mvm = new MainViewModel(ui.Object);
            mvm.OutputCommand.Execute(null);

            captured.Should().NotBeNull();
            captured.Should().Contain("Sin");
            captured.Should().Contain("Tan");
            captured.Should().Contain("Cube");
        }

        [Fact]
        public void UpdateElementCommand_OnSelectedItem_IncrementsN()
        {
            var ui = new Mock<IUIServices>();
            var mvm = new MainViewModel(ui.Object);
            int before = mvm.SelectedItem!.N;

            mvm.UpdateElementCommand.Execute(null);

            mvm.SelectedItem.N.Should().Be(before + 1);
        }

        [Fact]
        public void UpdateElementCommand_CannotExecute_WhenNoSelection()
        {
            var ui = new Mock<IUIServices>();
            var mvm = new MainViewModel(ui.Object, new DataCollection());
            mvm.SelectedItem = null;

            mvm.UpdateElementCommand.CanExecute(null).Should().BeFalse();
        }
    }
}
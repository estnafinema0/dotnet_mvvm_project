using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using TestDataLibrary;

namespace TestDataViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly DataCollection dataCollection;
        private readonly IUIServices uiServices;
        private DataItemViewModel? selectedItem;

        public ObservableCollection<DataItemViewModel> Items { get; }

        public DataItemViewModel? SelectedItem
        {
            get => selectedItem;
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    RaisePropertyChanged();
                }
            }
        }

        public RelayCommand UpdateCommand { get; }
        public RelayCommand OutputCommand { get; }
        public RelayCommand UpdateElementCommand { get; }

        public MainViewModel(IUIServices uiServices)
            : this(uiServices, new DataCollection()) { }

        public MainViewModel(IUIServices uiServices, DataCollection dataCollection)
        {
            this.uiServices = uiServices;
            this.dataCollection = dataCollection;

            Items = new ObservableCollection<DataItemViewModel>(
                this.dataCollection.Select(d => new DataItemViewModel(d)));

            this.dataCollection.CollectionChanged += OnCollectionChanged;

            UpdateCommand = new RelayCommand(_ => this.dataCollection.UpdateCollection());

            OutputCommand = new RelayCommand(
                _ => this.uiServices.ShowMessage(this.dataCollection.ToString(), "DataCollection"));

            UpdateElementCommand = new RelayCommand(
                _ => SelectedItem!.UpdateElementCommand.Execute(null),
                _ => SelectedItem != null);

            if (Items.Count > 0) SelectedItem = Items[0];
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (DataItem newItem in e.NewItems)
                    Items.Add(new DataItemViewModel(newItem));
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Items.Clear();
                foreach (var d in dataCollection)
                    Items.Add(new DataItemViewModel(d));
            }
        }
    }
}
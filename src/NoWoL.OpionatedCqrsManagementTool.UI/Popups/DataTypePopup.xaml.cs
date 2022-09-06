using System.Collections.ObjectModel;
using CodeGen.UI.Models;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CodeGen.UI.Popups
{
    public partial class DataTypePopup : Popup
    {
        public DataTypePopup(IReadOnlyList<IHasName> items)
        {
            InitializeComponent();

            BindingContext = new DataTypePopupViewModel(items);

            CanBeDismissedByTappingOutsideOfPopup = false;
        }

        private DataTypePopupViewModel TypedBindingContext => (BindingContext as DataTypePopupViewModel)!;

        public void OkClicked(object sender, EventArgs args)
        {
            Close(TypedBindingContext.SelectedDataType);
        }

        public void CancelClicked(object sender, EventArgs args)
        {
            Close();
        }
    }

    public partial class DataTypePopupViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<IHasName> _list = new();

        [ObservableProperty]
        private IHasName? _selectedDataType;

        [ObservableProperty]
        private string? _searchTerm;

        private readonly IReadOnlyList<IHasName> _originalList;

        public DataTypePopupViewModel(IReadOnlyList<IHasName> models)
        {
            _originalList = models;

            FilterModels(null);
        }

        partial void OnSearchTermChanged(string? value)
        {
            SelectedDataType = null;
            FilterModels(value);
        }

        private void FilterModels(string? searchTerm)
        {
            List.Clear();

            var query = _originalList.AsQueryable();

            if (!String.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x => x.Name != null && x.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            foreach (var item in query.OrderBy(x => x.Name))
            {
                List.Add(item);
            }
        }
    }
}
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Windows.Foundation.Collections;

namespace NoWoL.OpinionatedCqrsManagementTool.UI.Models.Maui
{
    public partial class ModelInfo : ObservableValidator, IHasName
    {
        public ModelInfo()
        {
            // workaround to correctly bind the IsVisible property of a StackLayout
            // https://github.com/xamarin/XamarinCommunityToolkit/issues/768
            
            Properties.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(Properties));
            EnumValues.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(EnumValues));
        }

        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private ModelLayerInfo _domain = new();

        [ObservableProperty]
        private ModelLayerInfo _service = new();

        [ObservableProperty]
        private ObservableCollection<ModelPropertyInfo> _properties = new();

        [ObservableProperty]
        private ObservableCollection<EnumValueModel> _enumValues = new();
    }
}
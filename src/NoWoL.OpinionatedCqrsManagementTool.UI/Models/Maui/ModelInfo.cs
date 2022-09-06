using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NoWoL.OpinionatedCqrsManagementTool.UI.Models.Maui
{
    public partial class ModelInfo : ObservableValidator, IHasName
    {
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
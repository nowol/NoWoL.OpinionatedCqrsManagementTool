using CommunityToolkit.Mvvm.ComponentModel;

namespace NoWoL.OpinionatedCqrsManagementTool.UI.Models.Maui
{
    public partial class EnumValueModel : ObservableValidator
    {
        [ObservableProperty]
        private string? _text;

        [ObservableProperty]
        private int? _value;
    }
}

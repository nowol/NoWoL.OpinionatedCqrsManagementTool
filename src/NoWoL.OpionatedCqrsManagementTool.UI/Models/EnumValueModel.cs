using CommunityToolkit.Mvvm.ComponentModel;

namespace CodeGen.UI.Models
{
    public partial class EnumValueModel : ObservableValidator
    {
        [ObservableProperty]
        private string? _text;

        [ObservableProperty]
        private int? _value;
    }
}

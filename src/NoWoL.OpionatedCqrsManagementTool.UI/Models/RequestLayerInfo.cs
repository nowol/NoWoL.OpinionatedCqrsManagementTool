using CommunityToolkit.Mvvm.ComponentModel;

namespace CodeGen.UI.Models
{
    public partial class RequestLayerInfo : ObservableValidator
    {
        [ObservableProperty]
        private string? _namespace;

        [ObservableProperty]
        private RequestInfo? _inheritsFrom;
    }
}
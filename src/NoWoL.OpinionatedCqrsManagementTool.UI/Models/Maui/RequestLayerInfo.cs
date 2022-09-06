using CommunityToolkit.Mvvm.ComponentModel;

namespace NoWoL.OpinionatedCqrsManagementTool.UI.Models.Maui
{
    public partial class RequestLayerInfo : ObservableValidator
    {
        [ObservableProperty]
        private string? _namespace;

        [ObservableProperty]
        private RequestInfo? _inheritsFrom;
    }
}
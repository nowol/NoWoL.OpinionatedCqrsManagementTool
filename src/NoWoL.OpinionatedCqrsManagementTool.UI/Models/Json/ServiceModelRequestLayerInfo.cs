using System.Collections.ObjectModel;
using NoWoL.OpinionatedCqrsManagementTool.UI.Models.Maui;

namespace NoWoL.OpinionatedCqrsManagementTool.UI.Models.Json
{
    public class ServiceModelRequestLayerInfo
    {
        public ServiceModelRequestLayerInfo()
        {
        }

        public ServiceModelRequestLayerInfo(RequestLayerInfo requestLayerInfo, ObservableCollection<RequestInfo> allRequests)
        {
            Namespace = requestLayerInfo.Namespace;
            InheritsFrom = requestLayerInfo.InheritsFrom == null ? null : allRequests.FirstOrDefault(x => x == requestLayerInfo.InheritsFrom)?.Name;
        }

        public string? Namespace { get; set; }

        public string? InheritsFrom { get; set; }
    }
}
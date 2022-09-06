using System.Collections.ObjectModel;
using NoWoL.OpinionatedCqrsManagementTool.UI.Models.Maui;

namespace NoWoL.OpinionatedCqrsManagementTool.UI.Models.Json
{
    public class ServiceModelModelLayerInfo
    {
        public ServiceModelModelLayerInfo()
        {
        }

        public ServiceModelModelLayerInfo(ModelLayerInfo modelLayerInfo, ObservableCollection<ModelInfo> allModels)
        {
            Generate = modelLayerInfo.Generate;
            Namespace = modelLayerInfo.Namespace;
            OverriddenName = modelLayerInfo.OverriddenName;
            InheritsFrom = modelLayerInfo.InheritsFrom == null ? null : allModels.FirstOrDefault(x => x == modelLayerInfo.InheritsFrom)?.Name;
            InheritsGeneric = modelLayerInfo.InheritsGeneric == null ? null : allModels.FirstOrDefault(x => x == modelLayerInfo.InheritsGeneric)?.Name;
            Converter = modelLayerInfo.Converter;
        }

        public bool Generate { get; set; }

        public string? Namespace { get; set; }

        public string? OverriddenName { get; set; }

        public string? InheritsFrom { get; set; }

        public string? InheritsGeneric { get; set; }

        public string? Converter { get; set; }
    }
}
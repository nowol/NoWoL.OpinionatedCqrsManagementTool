using System.Collections.ObjectModel;
using NoWoL.OpinionatedCqrsManagementTool.UI.Models.Maui;

namespace NoWoL.OpinionatedCqrsManagementTool.UI.Models.Json
{
    public class ServiceModelModel
    {
        public ServiceModelModel()
        {
        }

        public ServiceModelModel(ModelInfo modelInfo, ObservableCollection<ModelInfo> allModels)
        {
            Name = modelInfo.Name;
            Domain = new ServiceModelModelLayerInfo(modelInfo.Domain, allModels);
            Service = new ServiceModelModelLayerInfo(modelInfo.Service, allModels);
            Properties = modelInfo.Properties.Select(x => new ServiceModelModelProperty(x, allModels)).ToList();

            if (modelInfo.EnumValues.Count > 0)
            {
                EnumValues = modelInfo.EnumValues.Select(x => new ServiceModelEnumValue
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();
            }
        }

        public string? Name { get; set; }

        public bool Generate { get; set; }

        public ServiceModelModelLayerInfo? Domain { get; set; }

        public ServiceModelModelLayerInfo? Service { get; set; }

        public List<ServiceModelModelProperty>? Properties { get; set; }

        public List<ServiceModelEnumValue>? EnumValues { get; set; }
    }
}
using System.Collections.ObjectModel;
using NoWoL.OpinionatedCqrsManagementTool.UI.Models.Maui;

namespace NoWoL.OpinionatedCqrsManagementTool.UI.Models.Json
{
    public class ServiceModelModelProperty
    {
        public ServiceModelModelProperty()
        {

        }

        public ServiceModelModelProperty(ModelPropertyInfo modelPropertyInfo, ObservableCollection<ModelInfo> allModels)
        {
            Name = modelPropertyInfo.Name;
            Description = modelPropertyInfo.Description;
            IsNullable = modelPropertyInfo.IsNullable;
            Initialize = modelPropertyInfo.Initialize;
            IsList = modelPropertyInfo.IsList;
            HideInLogs = modelPropertyInfo.HideInLogs;
            MaxLength = modelPropertyInfo.MaxLength;
            DataType = modelPropertyInfo.DataType == null ? null : allModels.FirstOrDefault(x => x == modelPropertyInfo.DataType)?.Name;
        }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool IsNullable { get; set; }

        public bool Initialize { get; set; }

        public bool IsList { get; set; }

        public bool HideInLogs { get; set; }

        public int? MaxLength { get; set; }

        public string? DataType { get; set; }
    }
}
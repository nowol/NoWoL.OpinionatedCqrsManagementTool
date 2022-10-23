using System.Collections.ObjectModel;
using NoWoL.OpinionatedCqrsManagementTool.UI.Models.Maui;

namespace NoWoL.OpinionatedCqrsManagementTool.UI.Models.Json
{
    public class ServiceModelRequestProperty
    {
        public ServiceModelRequestProperty()
        {

        }

        public ServiceModelRequestProperty(RequestPropertyInfo requestPropertyInfo, ObservableCollection<ModelInfo> allModels)
        {
            Name = requestPropertyInfo.Name;
            Description = requestPropertyInfo.Description;
            IsNullable = requestPropertyInfo.IsNullable;
            Initialize = requestPropertyInfo.Initialize;
            IsList = requestPropertyInfo.IsList;
            HideInLogs = requestPropertyInfo.HideInLogs;
            MaxLength = requestPropertyInfo.MaxLength;
            DataType = requestPropertyInfo.DataType == null ? null : allModels.FirstOrDefault(x => x == requestPropertyInfo.DataType)?.Name;
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
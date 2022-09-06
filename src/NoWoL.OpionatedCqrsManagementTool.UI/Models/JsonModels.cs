using System.Collections.ObjectModel;

namespace CodeGen.UI.Models
{

    public class ServiceModelJson
    {
        public List<ServiceModelModel>? Models { get; set; }
        public List<ServiceModelRequest>? Requests { get; set; }
    }

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
            MaxLength = modelPropertyInfo.MaxLength;
            DataType = modelPropertyInfo.DataType == null ? null : allModels.FirstOrDefault(x => x == modelPropertyInfo.DataType)?.Name;
        }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool IsNullable { get; set; }

        public bool Initialize { get; set; }

        public bool IsList { get; set; }

        public int? MaxLength { get; set; }

        public string? DataType { get; set; }
    }
    
    public partial class ServiceModelEnumValue
    {
        public string? Text { get; set; }
        
        public int? Value { get; set; }
    }

    public class ServiceModelRequest
    {
        public ServiceModelRequest()
        {
        }

        public ServiceModelRequest(RequestInfo requestInfo, ObservableCollection<ModelInfo> allModels, ObservableCollection<RequestInfo> allRequests)
        {
            Name = requestInfo.Name;
            Url = requestInfo.Url;
            Verb = requestInfo.Verb;
            Domain = new ServiceModelRequestLayerInfo(requestInfo.Domain, allRequests);
            Service = new ServiceModelRequestLayerInfo(requestInfo.Service, allRequests);
            Properties = requestInfo.Properties.Select(x => new ServiceModelRequestProperty(x, allModels)).ToList();
            ReturnCodes = requestInfo.ReturnCodes.Select(x => new ServiceModelRequestReturn(x, allModels)).ToList();
        }

        public string? Name { get; set; }

        public string? Url { get; set; }

        public string? Verb { get; set; }

        public bool RequiresAuthentication { get; set; }

        public ServiceModelRequestLayerInfo? Domain { get; set; }

        public ServiceModelRequestLayerInfo? Service { get; set; }

        public List<ServiceModelRequestProperty>? Properties { get; set; }

        public List<ServiceModelRequestReturn>? ReturnCodes { get; set; }
    }

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
            MaxLength = requestPropertyInfo.MaxLength;
            DataType = requestPropertyInfo.DataType == null ? null : allModels.FirstOrDefault(x => x == requestPropertyInfo.DataType)?.Name;
        }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool IsNullable { get; set; }

        public bool Initialize { get; set; }

        public bool IsList { get; set; }

        public int? MaxLength { get; set; }

        public string? DataType { get; set; }
    }

    public class ServiceModelRequestReturn
    {
        public ServiceModelRequestReturn()
        {

        }

        public ServiceModelRequestReturn(RequestReturnCode rrc, ObservableCollection<ModelInfo> allModels)
        {
            StatusCode = rrc.StatusCode;
            Returns = rrc.Returns == null ? null : allModels.FirstOrDefault(x => x == rrc.Returns)?.Name;
        }

        public int StatusCode { get; set; }

        public string? Returns { get; set; }
    }
}

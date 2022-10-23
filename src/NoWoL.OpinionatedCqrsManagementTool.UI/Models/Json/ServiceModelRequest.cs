using System.Collections.ObjectModel;
using System.Security.Claims;
using NoWoL.OpinionatedCqrsManagementTool.UI.Models.Maui;

namespace NoWoL.OpinionatedCqrsManagementTool.UI.Models.Json
{
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
            AllowAnonymous = requestInfo.AllowAnonymous;
            Claims = requestInfo.Claims;
            Domain = new ServiceModelRequestLayerInfo(requestInfo.Domain, allRequests);
            Service = new ServiceModelRequestLayerInfo(requestInfo.Service, allRequests);
            Properties = requestInfo.Properties.Select(x => new ServiceModelRequestProperty(x, allModels)).ToList();
            ReturnCodes = requestInfo.ReturnCodes.Select(x => new ServiceModelRequestReturn(x, allModels)).ToList();
        }

        public string? Name { get; set; }

        public string? Url { get; set; }

        public string? Verb { get; set; }

        public bool AllowAnonymous { get; set; }

        public string? Claims { get; set; }

        public ServiceModelRequestLayerInfo? Domain { get; set; }

        public ServiceModelRequestLayerInfo? Service { get; set; }

        public List<ServiceModelRequestProperty>? Properties { get; set; }

        public List<ServiceModelRequestReturn>? ReturnCodes { get; set; }
    }
}
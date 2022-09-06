using System.Collections.ObjectModel;
using NoWoL.OpinionatedCqrsManagementTool.UI.Models.Maui;

namespace NoWoL.OpinionatedCqrsManagementTool.UI.Models.Json
{
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
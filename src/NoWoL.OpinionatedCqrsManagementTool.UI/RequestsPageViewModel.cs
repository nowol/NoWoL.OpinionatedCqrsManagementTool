using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NoWoL.OpinionatedCqrsManagementTool.UI.Models;
using NoWoL.OpinionatedCqrsManagementTool.UI.Models.Maui;

namespace NoWoL.OpinionatedCqrsManagementTool.UI
{
    public partial class RequestsPageViewModel : ObservableObject
    {
        private readonly GeneratorConfiguration _generatorConfiguration;

        [ObservableProperty]
        private RequestInfo? _selectedRequest;

        [ObservableProperty]
        private string? _searchTerm;

        [ObservableProperty]
        private ObservableCollection<RequestInfo> _requests = new();

        public ObservableCollection<ModelInfo> Models => _generatorConfiguration.Models;

        public RequestsPageViewModel(GeneratorConfiguration generatorConfiguration)
        {
            _generatorConfiguration = generatorConfiguration;
            FilterRequests(null);
        }

        partial void OnSearchTermChanged(string? value)
        {
            SelectedRequest = null;
            FilterRequests(value);
        }

        private void FilterRequests(string? searchTerm)
        {
            Requests.Clear();

            IQueryable<RequestInfo> query = Queryable.AsQueryable<RequestInfo>(_generatorConfiguration.Requests);

            if (!String.IsNullOrWhiteSpace(searchTerm))
            {
                query = Queryable.Where(query,
                                        x => x.Name != null && x.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            foreach (var requestInfo in Queryable.OrderBy<RequestInfo, string?>(query,
                                                             x => x.Name))
            {
                Requests.Add(requestInfo);
            }
        }

        public void AddRequest(RequestInfo request)
        {
            _generatorConfiguration.Requests.Add(request);
            FilterRequests(SearchTerm);
        }

        public void RemoveRequest(RequestInfo request)
        {
            _generatorConfiguration.Requests.Remove(request);
            FilterRequests(SearchTerm);
        }

        public void Reset()
        {
            SelectedRequest = null;
            SearchTerm = null;
            FilterRequests(SearchTerm);
        }
    }
}
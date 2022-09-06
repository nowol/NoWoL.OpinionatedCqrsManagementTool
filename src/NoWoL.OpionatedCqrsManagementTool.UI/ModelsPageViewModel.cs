using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NoWoL.OpinionatedCqrsManagementTool.UI.Models;
using NoWoL.OpinionatedCqrsManagementTool.UI.Models.Maui;

namespace NoWoL.OpinionatedCqrsManagementTool.UI
{
    public partial class ModelsPageViewModel : ObservableObject
    {
        private readonly GeneratorConfiguration _generatorConfiguration;

        [ObservableProperty]
        private ModelInfo? _selectedModel;

        [ObservableProperty]
        private string? _searchTerm;

        [ObservableProperty]
        private ObservableCollection<ModelInfo> _models = new();

        public ObservableCollection<ModelInfo> AllModels => _generatorConfiguration.Models;

        public ModelsPageViewModel(GeneratorConfiguration generatorConfiguration)
        {
            _generatorConfiguration = generatorConfiguration;
            FilterModels(null);
        }

        partial void OnSearchTermChanged(string? value)
        {
            SelectedModel = null;
            FilterModels(value);
        }

        private void FilterModels(string? searchTerm)
        {
            Models.Clear();

            IQueryable<ModelInfo> query = Queryable.AsQueryable<ModelInfo>(_generatorConfiguration.Models);

            if (!String.IsNullOrWhiteSpace(searchTerm))
            {
                query = Queryable.Where(query,
                                        x => x.Name != null && x.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            foreach (var modelInfo in Queryable.OrderBy<ModelInfo, string?>(query,
                                                           x => x.Name))
            {
                Models.Add(modelInfo);
            }
        }

        public void AddModel(ModelInfo model)
        {
            _generatorConfiguration.Models.Add(model);
            FilterModels(SearchTerm);
        }

        public void RemoveModel(ModelInfo model)
        {
            _generatorConfiguration.Models.Remove(model);
            FilterModels(SearchTerm);
        }

        public void Reset()
        {
            SelectedModel = null;
            SearchTerm = null;
            FilterModels(SearchTerm);
        }
    }
}
using System.Collections.ObjectModel;
using CodeGen.UI.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CodeGen.UI
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

            IQueryable<ModelInfo> query = _generatorConfiguration.Models.AsQueryable();

            if (!String.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x => x.Name != null && x.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            foreach (var modelInfo in query.OrderBy(x => x.Name))
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
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoWoL.OpinionatedCqrsManagementTool.UI.Models;

namespace NoWoL.OpinionatedCqrsManagementTool.UI;

public partial class ConfigurationPage : ContentPage
{
    private readonly GeneratorConfiguration _cfg;

    public ConfigurationPage(GeneratorConfiguration cfg)
	{
        _cfg = cfg ?? throw new ArgumentNullException(nameof(cfg));
        InitializeComponent();

        BindingContext = new ConfigurationPageViewModel(cfg);
    }

    private ConfigurationPageViewModel TypedBindingContext => (BindingContext as ConfigurationPageViewModel)!;

    private async void SaveClicked(object? sender, EventArgs e)
    {
        await _cfg.Save().ConfigureAwait(true);
    }
}

public partial class ConfigurationPageViewModel : ObservableValidator
{
    public GeneratorConfiguration GeneratorConfiguration { get; }

    public ConfigurationPageViewModel(GeneratorConfiguration generatorConfiguration)
    {
        GeneratorConfiguration = generatorConfiguration ?? throw new ArgumentNullException(nameof(generatorConfiguration));
    }

    [RelayCommand]
    private async void Load()
    {
        await GeneratorConfiguration.Load().ConfigureAwait(true);
    }
}
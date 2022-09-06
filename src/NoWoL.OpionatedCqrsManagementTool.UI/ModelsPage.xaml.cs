using NoWoL.OpinionatedCqrsManagementTool.UI.Models;
using NoWoL.OpinionatedCqrsManagementTool.UI.Models.Maui;
using Button = Microsoft.Maui.Controls.Button;

namespace NoWoL.OpinionatedCqrsManagementTool.UI;

public partial class ModelsPage : ContentPage
{
	public ModelsPage(GeneratorConfiguration generatorConfiguration)
	{
		InitializeComponent();

        BindingContext = new ModelsPageViewModel(generatorConfiguration);

        generatorConfiguration.OnLoaded += GeneratorConfiguration_OnLoaded;
    }

    private ModelsPageViewModel TypedBindingContext => (BindingContext as ModelsPageViewModel)!;
  
    private void GeneratorConfiguration_OnLoaded()
    {
        TypedBindingContext.Reset();
    }
    
    public async void AddModel(object sender, EventArgs args)
    {
        string result = await DisplayPromptAsync("Question", "What's the name of your new type?").ConfigureAwait(true);

        if (String.IsNullOrWhiteSpace(result))
        {
            return;
        }

        var exists = TypedBindingContext.Models.Any(x => String.Equals(x.Name,
                                                                       result,
                                                                       StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            await DisplayAlert("Oh no",
                               $"Type '{result}' already exists",
                               "Close").ConfigureAwait(false);

            return;
        }

        var model = new ModelInfo { Name = result };
        model.Domain.Generate = true;
        model.Service.Generate = true;
        TypedBindingContext.AddModel(model);
        TypedBindingContext.SelectedModel = model;
    }

    public async void RemoveModel(object sender, EventArgs args)
    {
        var model = TypedBindingContext.SelectedModel;
    
        if (model == null)
        {
            return;
        }

        bool answer = await DisplayAlert("Question?", $"Are you sure you want to remove model '{model.Name}'?", "Yes", "No");

        if (!answer)
        {
            return;
        }

        TypedBindingContext.SelectedModel = null;
        TypedBindingContext.RemoveModel(model);
    }

    private async void AddProperty(object? sender, EventArgs e)
    {
        var ctx = TypedBindingContext;

        if (ctx.SelectedModel == null)
        {
            return;
        }

        string result = await DisplayPromptAsync("Question", "What's the name of your new property?").ConfigureAwait(true);

        if (String.IsNullOrWhiteSpace(result))
        {
            return;
        }

        var exists = ctx.SelectedModel.Properties.Any(x => String.Equals(x.Name,
                                                                         result,
                                                                         StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            await DisplayAlert("Oh no",
                               $"Property '{result}' already exists",
                               "Close").ConfigureAwait(false);

            return;
        }

        ctx.SelectedModel.Properties.Add(CreateModelPropertyInfoFromName(result));
    }

    private ModelPropertyInfo CreateModelPropertyInfoFromName(string result)
    {
        var parts = result.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

        var mi = new ModelPropertyInfo { Name = parts[0] };

        if (parts.Length > 1)
        {
            var dataTypeName = parts[1];
            mi.IsNullable = dataTypeName.EndsWith('?');

            if (mi.IsNullable)
            {
                dataTypeName = dataTypeName.TrimEnd('?');
            }

            mi.DataType = TypedBindingContext.AllModels.FirstOrDefault(x => String.Equals(x.Name, dataTypeName, StringComparison.OrdinalIgnoreCase));
        }

        return mi;
    }

    public async void RemoveProperty(object sender, EventArgs args)
    {
        if (TypedBindingContext.SelectedModel == null)
        {
            return;
        }

        var btn = (Button)sender;
        var property = (ModelPropertyInfo)btn.BindingContext;

        bool answer = await DisplayAlert("Question?", $"Are you sure you want to remove property '{property.Name}'?", "Yes", "No");

        if (!answer)
        {
            return;
        }
        
        TypedBindingContext.SelectedModel.Properties.Remove(property);
    }

    public void MovePropertyUp(object sender, EventArgs args)
    {
        if (TypedBindingContext.SelectedModel == null)
        {
            return;
        }

        var btn = (Button)sender;
        var property = (ModelPropertyInfo)btn.BindingContext;

        var index = TypedBindingContext.SelectedModel.Properties.IndexOf(property);

        if (index > 0)
        {
            TypedBindingContext.SelectedModel.Properties.RemoveAt(index);
            TypedBindingContext.SelectedModel.Properties.Insert(index - 1,
                                                                property);
        }
    }

    public void MovePropertyDown(object sender, EventArgs args)
    {
        if (TypedBindingContext.SelectedModel == null)
        {
            return;
        }

        var btn = (Button)sender;
        var property = (ModelPropertyInfo)btn.BindingContext;

        var index = TypedBindingContext.SelectedModel.Properties.IndexOf(property);

        if (index + 1 < TypedBindingContext.SelectedModel.Properties.Count)
        {
            TypedBindingContext.SelectedModel.Properties.RemoveAt(index);
            TypedBindingContext.SelectedModel.Properties.Insert(index + 1,
                                                                property);
        }
    }

    private async void RenameModel(object? sender, EventArgs e)
    {
        var ctx = TypedBindingContext;

        if (ctx.SelectedModel == null)
        {
            return;
        }

        string result = await DisplayPromptAsync("Question", "What's the new name for the model?").ConfigureAwait(true);

        if (String.IsNullOrWhiteSpace(result))
        {
            return;
        }

        if (ctx.Models.Any(x => String.Equals(x.Name,
                                              result,
                                              StringComparison.OrdinalIgnoreCase)))
        {
            await DisplayAlert("Oh no",
                               "This name is taken",
                               "Close").ConfigureAwait(false);
        }

        ctx.SelectedModel.Name = result;
    }

    private async void AddEnumValue(object? sender, EventArgs e)
    {
        var ctx = TypedBindingContext;

        if (ctx.SelectedModel == null)
        {
            return;
        }

        string result = await DisplayPromptAsync("Question", "What's the new value?").ConfigureAwait(true);

        if (String.IsNullOrWhiteSpace(result))
        {
            return;
        }

        var exists = ctx.SelectedModel.EnumValues.Any(x => String.Equals(x.Text,
                                                                         result,
                                                                         StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            await DisplayAlert("Oh no",
                               $"Enum value '{result}' already exists",
                               "Close").ConfigureAwait(false);

            return;
        }

        ctx.SelectedModel.EnumValues.Add(new EnumValueModel
                                         {
                                             Text = result
                                         });
    }

    public async void RemoveEnumValue(object sender, EventArgs args)
    {
        if (TypedBindingContext.SelectedModel == null)
        {
            return;
        }

        var btn = (Button)sender;
        var value = (EnumValueModel)btn.BindingContext;

        bool answer = await DisplayAlert("Question?", $"Are you sure you want to enum value '{value.Text}'?", "Yes", "No");

        if (!answer)
        {
            return;
        }
        
        TypedBindingContext.SelectedModel.EnumValues.Remove(value);
    }

    public void MoveEnumValueUp(object sender, EventArgs args)
    {
        if (TypedBindingContext.SelectedModel == null)
        {
            return;
        }

        var btn = (Button)sender;
        var value = (EnumValueModel)btn.BindingContext;

        var index = TypedBindingContext.SelectedModel.EnumValues.IndexOf(value);

        if (index > 0)
        {
            TypedBindingContext.SelectedModel.EnumValues.RemoveAt(index);
            TypedBindingContext.SelectedModel.EnumValues.Insert(index - 1,
                                                                value);
        }
    }

    public void MoveEnumValueDown(object sender, EventArgs args)
    {
        if (TypedBindingContext.SelectedModel == null)
        {
            return;
        }

        var btn = (Button)sender;
        var value = (EnumValueModel)btn.BindingContext;

        var index = TypedBindingContext.SelectedModel.EnumValues.IndexOf(value);

        if (index + 1 < TypedBindingContext.SelectedModel.EnumValues.Count)
        {
            TypedBindingContext.SelectedModel.EnumValues.RemoveAt(index);
            TypedBindingContext.SelectedModel.EnumValues.Insert(index + 1,
                                                                value);
        }
    }
}
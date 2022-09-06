using CodeGen.UI.Models;

namespace CodeGen.UI
{
    public partial class RequestsPage : ContentPage
    {
        public RequestsPage(GeneratorConfiguration generatorConfiguration)
        {
            InitializeComponent();

            BindingContext = new RequestsPageViewModel(generatorConfiguration);

            generatorConfiguration.OnLoaded += GeneratorConfiguration_OnLoaded;
        }

        private RequestsPageViewModel TypedBindingContext => (BindingContext as RequestsPageViewModel)!;

        private void GeneratorConfiguration_OnLoaded()
        {
            TypedBindingContext.Reset();
        }

        public async void AddRequest(object sender, EventArgs args)
        {
            string result = await DisplayPromptAsync("Question", "What's the name of your new request?").ConfigureAwait(true);

            if (String.IsNullOrWhiteSpace(result))
            {
                return;
            }

            var exists = TypedBindingContext.Requests.Any(x => String.Equals(x.Name,
                                                                             result,
                                                                             StringComparison.OrdinalIgnoreCase));

            if (exists)
            {
                await DisplayAlert("Oh no",
                                   $"Type '{result}' already exists",
                                   "Close").ConfigureAwait(false);

                return;
            }

            var request = new RequestInfo { Name = result };
            TypedBindingContext.AddRequest(request);
            TypedBindingContext.SelectedRequest = request;
        }

        public async void RemoveRequest(object sender, EventArgs args)
        {
            var request = TypedBindingContext.SelectedRequest;

            if (request == null)
            {
                return;
            }

            bool answer = await DisplayAlert("Question?", $"Are you sure you want to remove request '{request.Name}'?", "Yes", "No");

            if (!answer)
            {
                return;
            }

            TypedBindingContext.SelectedRequest = null;
            TypedBindingContext.RemoveRequest(request);
        }

        private async void AddProperty(object? sender, EventArgs e)
        {
            var ctx = TypedBindingContext;

            if (ctx.SelectedRequest == null)
            {
                return;
            }

            string result = await DisplayPromptAsync("Question", "What's the name of your new property?").ConfigureAwait(true);

            if (String.IsNullOrWhiteSpace(result))
            {
                return;
            }

            var exists = ctx.SelectedRequest.Properties.Any(x => String.Equals(x.Name,
                                                                               result,
                                                                               StringComparison.OrdinalIgnoreCase));

            if (exists)
            {
                await DisplayAlert("Oh no",
                                   $"Property '{result}' already exists",
                                   "Close").ConfigureAwait(false);

                return;
            }

            ctx.SelectedRequest.Properties.Add(new RequestPropertyInfo
                                               {
                                                   Name = result
                                               });
        }

        public async void RemoveProperty(object sender, EventArgs args)
        {
            if (TypedBindingContext.SelectedRequest == null)
            {
                return;
            }

            var btn = (Button)sender;
            var property = (RequestPropertyInfo)btn.BindingContext;

            bool answer = await DisplayAlert("Question?", $"Are you sure you want to remove property '{property.Name}'?", "Yes", "No");

            if (!answer)
            {
                return;
            }

            TypedBindingContext.SelectedRequest.Properties.Remove(property);
        }

        public void MovePropertyUp(object sender, EventArgs args)
        {
            if (TypedBindingContext.SelectedRequest == null)
            {
                return;
            }

            var btn = (Button)sender;
            var property = (RequestPropertyInfo)btn.BindingContext;

            var index = TypedBindingContext.SelectedRequest.Properties.IndexOf(property);

            if (index > 0)
            {
                TypedBindingContext.SelectedRequest.Properties.RemoveAt(index);
                TypedBindingContext.SelectedRequest.Properties.Insert(index - 1,
                                                                      property);
            }
        }

        public void MovePropertyDown(object sender, EventArgs args)
        {
            if (TypedBindingContext.SelectedRequest == null)
            {
                return;
            }

            var btn = (Button)sender;
            var property = (RequestPropertyInfo)btn.BindingContext;

            var index = TypedBindingContext.SelectedRequest.Properties.IndexOf(property);

            if (index + 1 < TypedBindingContext.SelectedRequest.Properties.Count)
            {
                TypedBindingContext.SelectedRequest.Properties.RemoveAt(index);
                TypedBindingContext.SelectedRequest.Properties.Insert(index + 1,
                                                                      property);
            }
        }

        private async void RenameRequest(object? sender, EventArgs e)
        {
            var ctx = TypedBindingContext;

            if (ctx.SelectedRequest == null)
            {
                return;
            }

            string result = await DisplayPromptAsync("Question", "What's the new name for the request?").ConfigureAwait(true);

            if (String.IsNullOrWhiteSpace(result))
            {
                return;
            }

            if (ctx.Requests.Any(x => String.Equals(x.Name,
                                                    result,
                                                    StringComparison.OrdinalIgnoreCase)))
            {
                await DisplayAlert("Oh no",
                                   "This name is taken",
                                   "Close").ConfigureAwait(false);
            }

            ctx.SelectedRequest.Name = result;
        }

        private async void AddReturnCode(object? sender, EventArgs e)
        {
            var ctx = TypedBindingContext;

            if (ctx.SelectedRequest == null)
            {
                return;
            }

            string results = await DisplayPromptAsync("Question", "What's the new status code?").ConfigureAwait(true);

            foreach (var result in (results ?? String.Empty).Split(',',
                                                                   StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()))
            {

                if (!int.TryParse(result,
                                  out var statusCode)
                    || statusCode <= 0)
                {
                    return;
                }

                var exists = ctx.SelectedRequest.ReturnCodes.Any(x => x.StatusCode == statusCode);

                if (exists)
                {
                    await DisplayAlert("Oh no",
                                       $"Status code '{statusCode}' already exists",
                                       "Close").ConfigureAwait(false);

                    return;
                }

                ModelInfo? dataType = null;

                if (statusCode == 422)
                {
                    dataType = ctx.Models.FirstOrDefault(x => String.Equals(x.Name,
                                                                            "ValidationResult",
                                                                            StringComparison.OrdinalIgnoreCase));
                }
                else if (statusCode == 404)
                {
                    dataType = ctx.Models.FirstOrDefault(x => String.Equals(x.Name,
                                                                            "void",
                                                                            StringComparison.OrdinalIgnoreCase));
                }

                ctx.SelectedRequest.ReturnCodes.Add(new RequestReturnCode
                                                    {
                                                        StatusCode = statusCode,
                                                        Returns = dataType
                                                    });
            }
        }

        public async void RemoveReturnCode(object sender, EventArgs args)
        {
            if (TypedBindingContext.SelectedRequest == null)
            {
                return;
            }

            var btn = (Button)sender;
            var returnCode = (RequestReturnCode)btn.BindingContext;

            bool answer = await DisplayAlert("Question?", $"Are you sure you want to remove status code '{returnCode.StatusCode}'?", "Yes", "No");

            if (!answer)
            {
                return;
            }

            TypedBindingContext.SelectedRequest.ReturnCodes.Remove(returnCode);
        }

        public void MoveReturnCodeUp(object sender, EventArgs args)
        {
            if (TypedBindingContext.SelectedRequest == null)
            {
                return;
            }

            var btn = (Button)sender;
            var returnCode = (RequestReturnCode)btn.BindingContext;

            var index = TypedBindingContext.SelectedRequest.ReturnCodes.IndexOf(returnCode);

            if (index > 0)
            {
                TypedBindingContext.SelectedRequest.ReturnCodes.RemoveAt(index);
                TypedBindingContext.SelectedRequest.ReturnCodes.Insert(index - 1,
                                                                       returnCode);
            }
        }

        public void MoveReturnCodeDown(object sender, EventArgs args)
        {
            if (TypedBindingContext.SelectedRequest == null)
            {
                return;
            }

            var btn = (Button)sender;
            var returnCode = (RequestReturnCode)btn.BindingContext;

            var index = TypedBindingContext.SelectedRequest.ReturnCodes.IndexOf(returnCode);

            if (index + 1 < TypedBindingContext.SelectedRequest.ReturnCodes.Count)
            {
                TypedBindingContext.SelectedRequest.ReturnCodes.RemoveAt(index);
                TypedBindingContext.SelectedRequest.ReturnCodes.Insert(index + 1,
                                                                       returnCode);
            }
        }
    }
}
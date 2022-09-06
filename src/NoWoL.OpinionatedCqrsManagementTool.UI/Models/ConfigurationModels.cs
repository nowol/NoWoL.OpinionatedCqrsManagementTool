using System.Collections.ObjectModel;
using Windows.Storage.Pickers;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Platform;
using Newtonsoft.Json;
using NoWoL.OpinionatedCqrsManagementTool.UI.Models.Json;
using NoWoL.OpinionatedCqrsManagementTool.UI.Models.Maui;

namespace NoWoL.OpinionatedCqrsManagementTool.UI.Models
{
    public delegate void OnLoadedEventHandler();
   
    public partial class GeneratorConfiguration : ObservableValidator
    {
        public event OnLoadedEventHandler? OnLoaded;

        [ObservableProperty]
        private ObservableCollection<ModelInfo> _models = new();

        [ObservableProperty]
        private ObservableCollection<RequestInfo> _requests = new();

        public GeneratorConfiguration()
        {
            Reset();
        }

        public void Reset()
        {
            foreach (var type in new[]{
                                          "string",
                                          "int",
                                          "long",
                                          "decimal",
                                          "double",
                                          "float",
                                          "bool",
                                          "void",
                                      })
            {
                var mi3 = new ModelInfo
                          {
                              Name = type
                          };
                mi3.Domain.Generate = false;
                mi3.Service.Generate = false;
                Models.Add(mi3);
            }

            //foreach (var type in new[]{
            //                              typeof(string),
            //                          })
            //{
            //    var mi3 = new ModelInfo
            //              {
            //                  Name = type.FullName,
            //                  Generate = false
            //              };
            //    Models.Add(mi3);
            //}
        }

        public async Task Load()
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>> { { DevicePlatform.WinUI, new[] { "json" } } })
            });

            if (result != null)
            {
                if (!System.IO.File.Exists(result.FullPath))
                {
                    throw new FileNotFoundException("Could not open configuration file",
                                                    result.FullPath);
                }

                var content = await System.IO.File.ReadAllTextAsync(result.FullPath).ConfigureAwait(true);
                var json = JsonConvert.DeserializeObject<ServiceModelJson>(content);

                Reset();

                if (json == null)
                {
                    return;
                }

                PopulateModelsFromJson(json);
                PopulateRequestsFromJson(json);

                OnLoaded?.Invoke();
            }
        }

        private void PopulateRequestsFromJson(ServiceModelJson json)
        {
            Requests.Clear();

            if (json.Requests != null)
            {
                foreach (var serviceModelRequest in json.Requests)
                {
                    var ri = new RequestInfo();
                    ri.Name = serviceModelRequest.Name;
                    ri.Url = serviceModelRequest.Url;
                    ri.Verb = serviceModelRequest.Verb;
                    ri.RequiresAuthentication = serviceModelRequest.RequiresAuthentication;
                    ri.Domain.Namespace = serviceModelRequest.Domain?.Namespace;
                    ri.Service.Namespace = serviceModelRequest.Service?.Namespace;

                    if (serviceModelRequest.Properties != null)
                    {
                        foreach (var property in serviceModelRequest.Properties)
                        {
                            var prop = new RequestPropertyInfo();

                            prop.Name = property.Name;
                            prop.Description = property.Description;
                            prop.IsNullable = property.IsNullable;
                            prop.Initialize = property.Initialize;
                            prop.IsList = property.IsList;
                            prop.MaxLength = property.MaxLength;

                            ri.Properties.Add(prop);
                        }
                    }

                    if (serviceModelRequest.ReturnCodes != null)
                    {
                        foreach (var serviceModelRequestReturn in serviceModelRequest.ReturnCodes)
                        {
                            var rrc = new RequestReturnCode
                                      {
                                          StatusCode = serviceModelRequestReturn.StatusCode,
                                          Returns = GetDataTypeFromName((ObservableCollection<ModelInfo>)Models, serviceModelRequestReturn.Returns)
                            };
                            ri.ReturnCodes.Add(rrc);
                        }
                    }

                    Requests.Add(ri);
                }

                foreach (var requestInfo in Requests)
                {
                    var jsonRequest = json.Requests.First(x => String.Equals(x.Name, requestInfo.Name, StringComparison.OrdinalIgnoreCase));

                    requestInfo.Domain.InheritsFrom = GetDataTypeFromName((ObservableCollection<RequestInfo>)Requests, jsonRequest.Domain?.InheritsFrom);
                    requestInfo.Service.InheritsFrom = GetDataTypeFromName((ObservableCollection<RequestInfo>)Requests, jsonRequest.Service?.InheritsFrom);

                    foreach (var requestPropertyInfo in requestInfo.Properties)
                    {
                        var jsonProp = jsonRequest.Properties?.First(x => String.Equals(x.Name, requestPropertyInfo.Name, StringComparison.OrdinalIgnoreCase))!;
                        requestPropertyInfo.DataType = GetDataTypeFromName((ObservableCollection<ModelInfo>)Models, jsonProp.DataType);
                    }
                }
            }
        }

        private void PopulateModelsFromJson(ServiceModelJson json)
        {
            Models.Clear();

            if (json.Models != null)
            {
                foreach (var serviceModelModel in json.Models)
                {
                    var mi = new ModelInfo();
                    mi.Name = serviceModelModel.Name;
                    mi.Domain.Generate = serviceModelModel.Domain?.Generate ?? false;
                    mi.Domain.Namespace = serviceModelModel.Domain?.Namespace;
                    mi.Domain.OverriddenName = serviceModelModel.Domain?.OverriddenName;
                    mi.Domain.Converter = serviceModelModel.Domain?.Converter;
                    mi.Service.Generate = serviceModelModel.Service?.Generate ?? false;
                    mi.Service.Namespace = serviceModelModel.Service?.Namespace;
                    mi.Service.OverriddenName = serviceModelModel.Service?.OverriddenName;
                    mi.Service.Converter = serviceModelModel.Service?.Converter;

                    if (serviceModelModel.EnumValues != null)
                    {
                        foreach (var value in serviceModelModel.EnumValues)
                        {
                            mi.EnumValues.Add(new EnumValueModel
                                              {
                                                  Text = value.Text,
                                                  Value = value.Value
                                              });
                        }
                    }

                    if (serviceModelModel.Properties != null)
                    {
                        foreach (var property in serviceModelModel.Properties)
                        {
                            var prop = new ModelPropertyInfo();

                            prop.Name = property.Name;
                            prop.Description = property.Description;
                            prop.IsNullable = property.IsNullable;
                            prop.Initialize = property.Initialize;
                            prop.IsList = property.IsList;
                            prop.MaxLength = property.MaxLength;

                            mi.Properties.Add(prop);
                        }
                    }

                    Models.Add(mi);
                }

                foreach (var modelInfo in Models)
                {
                    var jsonModel = json.Models.First(x => String.Equals(x.Name, modelInfo.Name, StringComparison.OrdinalIgnoreCase));

                    modelInfo.Domain.InheritsFrom = GetDataTypeFromName((ObservableCollection<ModelInfo>)Models, jsonModel.Domain?.InheritsFrom);
                    modelInfo.Domain.InheritsGeneric = GetDataTypeFromName((ObservableCollection<ModelInfo>)Models, jsonModel.Domain?.InheritsGeneric);
                    modelInfo.Service.InheritsFrom = GetDataTypeFromName((ObservableCollection<ModelInfo>)Models, jsonModel.Service?.InheritsFrom);
                    modelInfo.Service.InheritsGeneric = GetDataTypeFromName((ObservableCollection<ModelInfo>)Models, jsonModel.Service?.InheritsGeneric);

                    foreach (var modelInfoProperty in modelInfo.Properties)
                    {
                        var jsonProp = jsonModel.Properties?.First(x => String.Equals(x.Name, modelInfoProperty.Name, StringComparison.OrdinalIgnoreCase))!;
                        modelInfoProperty.DataType = GetDataTypeFromName((ObservableCollection<ModelInfo>)Models, jsonProp.DataType);
                    }
                }
            }
        }

        private ModelInfo? GetDataTypeFromName(ObservableCollection<ModelInfo> list, string? modelName)
        {
            if (String.IsNullOrWhiteSpace(modelName))
            {
                return null;
            }

            return list.FirstOrDefault(x => String.Equals(x.Name, modelName, StringComparison.OrdinalIgnoreCase));
        }

        private RequestInfo? GetDataTypeFromName(ObservableCollection<RequestInfo> list, string? requestName)
        {
            if (String.IsNullOrWhiteSpace(requestName))
            {
                return null;
            }

            return list.FirstOrDefault(x => String.Equals(x.Name, requestName, StringComparison.OrdinalIgnoreCase));
        }

        public async Task Save()
        {
            var picker = new FileSavePicker();

            var hwnd = WindowStateManager.Default.GetActiveWindow()!.GetWindowHandle();
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            picker.FileTypeChoices.Add("JSON", new List<string>() { ".json" });
            picker.FileTypeChoices.Add("All files (*.*)", new List<string>() { "." });
            var file = await picker.PickSaveFileAsync();

            if (file != null)
            {
                var content = GenerateJsonContent();
                await System.IO.File.WriteAllTextAsync(file.Path,
                                                       content).ConfigureAwait(true);
            }
        }

        private string GenerateJsonContent()
        {
            var json = new ServiceModelJson();
            json.Models = ConvertModelsForSaving();
            json.Requests = ConvertRequestsForSaving();

            var content = JsonConvert.SerializeObject(json, Formatting.Indented);

            return content;
        }

        private List<ServiceModelRequest> ConvertRequestsForSaving()
        {
            return Enumerable.Select<RequestInfo, ServiceModelRequest>(Requests,
                                        x => new ServiceModelRequest(x, Models, Requests)).ToList();
        }

        private List<ServiceModelModel> ConvertModelsForSaving()
        {
            return Enumerable.Select<ModelInfo, ServiceModelModel>(Models,
                                        x => new ServiceModelModel(x, Models)).ToList();
        }
    }
}

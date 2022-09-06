using CommunityToolkit.Maui;
using CodeGen.UI.Models;

namespace CodeGen.UI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            var content = System.IO.File.ReadAllText(@"D:\Programmation\Net\ArtificialMemory3\src\CodeGen\tt\ServiceModel.json");

            ////builder.Services.AddSingleton
            //builder.Services.AddSingleton<IGeneratorSettings>((sp) =>
            //                                                  {
            //                                                      var settings = new GeneratorSettings
            //                                                      {
            //                                                          DomainNamespace = "ArtificialMemory.Domain",
            //                                                          ServiceNamespace = "ArtificialMemory.ServiceModel",
            //                                                      };

            //                                                      return settings;
            //                                                  });


            builder.Services.AddSingleton<GeneratorConfiguration>();

            //builder.Services.AddSingleton<IGeneratorSettings, GeneratorSettings>();
            builder.Services.AddTransient<ConfigurationPage>();
            builder.Services.AddTransient<ModelsPage>();
            builder.Services.AddTransient<RequestsPage>();

            return builder.Build();
        }
    }
}
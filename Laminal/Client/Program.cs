using Laminal.Client;
using Laminal.Shared.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Stl.Fusion;
using Stl.Fusion.Blazor;
using Stl.Fusion.Client;

namespace Laminal.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            var baseUri = new Uri(builder.HostEnvironment.BaseAddress);

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = baseUri });

            var fusion = builder.Services.AddFusion();
            fusion.AddBlazorUIServices();
            var fusionClient = fusion.AddRestEaseClient();
            fusionClient.ConfigureWebSocketChannel(c => new()
            {
                BaseUri = baseUri,
                LogLevel = LogLevel.Information,
                MessageLogLevel = LogLevel.None,
            });
            fusionClient.ConfigureHttpClient((c, name, o) => {
                var isFusionClient = (name ?? "").StartsWith("Stl.Fusion");
                var clientBaseUri = isFusionClient ? baseUri : new Uri($"{baseUri}api/");
                o.HttpClientActions.Add(client => client.BaseAddress = clientBaseUri);
            });

            // Fusion service clients
            fusionClient.AddReplicaService<ITaskService, ITaskClientDef>();

            builder.Services.AddMudServices();

            await builder.Build().RunAsync();
        }
    }
}
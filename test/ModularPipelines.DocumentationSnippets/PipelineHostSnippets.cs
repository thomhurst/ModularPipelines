using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines;
using ModularPipelines.Extensions;
using ModularPipelines.Options;

namespace ModularPipelines.DocumentationSnippets.PipelineHostExample;

// docs/docs/how-to/pipeline-host.md: Complete Example
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Pipeline.CreateBuilder(args);

        // Configuration
        builder.Configuration
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables();

        // Options
        builder.ConfigureOptions(options => options with
        {
            FailureMode = FailureMode.FailFast,
            IgnoreCategories = ["Experimental"],
        });

        // Services
        builder.Services.Configure<NuGetSettings>(builder.Configuration.GetSection("NuGet"));
        builder.Services.Configure<PublishSettings>(builder.Configuration.GetSection("Publish"));

        // Conditional modules
        if (builder.Environment.IsDevelopment())
        {
            builder
                .AddModule<LocalBuildModule>()
                .AddModule<LocalTestModule>();
        }
        else
        {
            builder
                .AddModule<CIBuildModule>()
                .AddModule<CITestModule>()
                .AddModule<PublishModule>();
        }

        // Always-registered modules
        builder
            .AddModule<CleanupModule>()
            .AddModule<ReportModule>();

        // Run
        await builder.RunAsync();
    }
}

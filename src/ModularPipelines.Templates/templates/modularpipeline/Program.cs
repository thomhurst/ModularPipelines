using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines;
using ModularPipelines.Extensions;
using TemplatePipeline.Modules;
using TemplatePipeline.Settings;

var pipelineDirectory = FindPipelineDirectory();
Environment.CurrentDirectory = pipelineDirectory;

var builder = Pipeline.CreateBuilder(args);

builder.Configuration
    .AddJsonFile(Path.Combine(pipelineDirectory, "appsettings.json"), optional: false)
    .AddEnvironmentVariables();

builder.Services.Configure<BuildSettings>(builder.Configuration.GetSection("Build"));

builder
    .AddModule<RestoreModule>()
    .AddModule<BuildModule>()
    .AddModule<TestModule>()
    .AddModule<PublishModule>();

await builder.ExecutePipelineAsync();

static string FindPipelineDirectory()
{
    for (var directory = Directory.GetParent(AppContext.BaseDirectory);
         directory is not null;
         directory = directory.Parent)
    {
        if (File.Exists(Path.Combine(directory.FullName, "appsettings.json"))
            && directory.EnumerateFiles("*.csproj").Any())
        {
            return directory.FullName;
        }
    }

    return AppContext.BaseDirectory;
}

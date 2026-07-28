using System.Runtime.CompilerServices;
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

static string FindPipelineDirectory([CallerFilePath] string sourceFilePath = "")
{
    var sourceDirectory = Path.GetDirectoryName(sourceFilePath);
    if (sourceDirectory is not null && IsPipelineDirectory(sourceDirectory))
    {
        return sourceDirectory;
    }

    for (var directory = new DirectoryInfo(AppContext.BaseDirectory);
         directory is not null;
         directory = directory.Parent)
    {
        if (IsPipelineDirectory(directory.FullName))
        {
            return directory.FullName;
        }
    }

    return AppContext.BaseDirectory;
}

static bool IsPipelineDirectory(string? directory) =>
    directory is not null
    && File.Exists(Path.Combine(directory, "appsettings.json"))
    && Directory.EnumerateFiles(directory, "*.csproj").Any();

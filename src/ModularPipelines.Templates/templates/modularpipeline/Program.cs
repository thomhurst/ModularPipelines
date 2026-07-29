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
    const string directoryVariable = "MODULAR_PIPELINES_DIRECTORY";
    var configuredDirectory = Environment.GetEnvironmentVariable(directoryVariable);
    if (!string.IsNullOrWhiteSpace(configuredDirectory))
    {
        var fullPath = Path.GetFullPath(configuredDirectory);
        if (IsPipelineDirectory(fullPath))
        {
            return fullPath;
        }

        throw new InvalidOperationException(
            $"{directoryVariable} must point to a directory containing appsettings.json and a project file.");
    }

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

    throw new InvalidOperationException(
        $"Could not locate the pipeline project directory. Set {directoryVariable} to its path.");
}

static bool IsPipelineDirectory(string? directory) =>
    directory is not null
    && Directory.Exists(directory)
    && File.Exists(Path.Combine(directory, "appsettings.json"))
    && Directory.EnumerateFiles(directory, "*.csproj").Any();

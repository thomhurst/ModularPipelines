using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines;
using ModularPipelines.Extensions;
using TemplatePipeline;
using TemplatePipeline.Modules;
using TemplatePipeline.Settings;

var pipelineDirectory = PipelineProjectDirectory.Find();
Environment.CurrentDirectory = pipelineDirectory;

// PipelineBuilder became disposable in v4; retain compatibility with earlier package versions.
var builder = Pipeline.CreateBuilder(args);
using var builderLifetime = ((object)builder) as IDisposable;

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

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines;
using ModularPipelines.Extensions;
using TemplatePipeline;
using TemplatePipeline.Modules;
using TemplatePipeline.Settings;

var builder = Pipeline.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables();

builder.Services.Configure<BuildSettings>(builder.Configuration.GetSection("Build"));

builder
    .AddModule<RestoreModule>()
    .AddModule<BuildModule>()
    .AddModule<TestModule>()
    .AddModule<PublishModule>();

await builder.ExecutePipelineAsync();

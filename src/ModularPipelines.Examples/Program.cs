using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines;
using ModularPipelines.Examples;
using ModularPipelines.Examples.Modules;
using ModularPipelines.Extensions;
using ModularPipelines.Options;

var builder = Pipeline.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

builder.Options.ExecutionMode = ExecutionMode.StopOnFirstException;
builder.Options.IgnoreCategories = ["Ignore"];

builder.Services.Configure<MyOptions>(builder.Configuration);

builder.Services
    .AddModule<SuccessModule>()
    .AddModule<LogSecretModule>()
    .AddModule<DependentOnSuccessModule>()
    .AddModule<DependentOn2>()
    .AddModule<DependentOn3>()
    .AddModule<DependentOn4>()
    .AddModule<SubmodulesModule>()
    .AddModule<GitVersionModule>()
    .AddModule<GitLastCommitModule>();

await builder.Build().RunAsync();

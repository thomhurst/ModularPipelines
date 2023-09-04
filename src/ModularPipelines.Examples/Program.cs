using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Examples;
using ModularPipelines.Examples.Modules;
using ModularPipelines.Extensions;
using ModularPipelines.Host;
using ModularPipelines.Options;

var modules = await PipelineHostBuilder.Create()
    .ConfigureAppConfiguration((context, builder) =>
    {
        builder.AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
            .AddUserSecrets<Program>()
            .AddCommandLine(args)
            .AddEnvironmentVariables();
    })
    .ConfigurePipelineOptions((context, options) =>
    {
        options.ExecutionMode = ExecutionMode.StopOnFirstException;
        options.IgnoreCategories = new[] { "Ignore" };
    })
    .ConfigureServices((context, collection) =>
    {
        collection.Configure<MyOptions>(context.Configuration);

        collection.AddModule<SuccessModule>()
            .AddModule<LogSecretModule>()
            .AddModule<DependentOnSuccessModule>()
            .AddModule<DependentOn2>()
            .AddModule<DependentOn3>()
            .AddModule<DependentOn4>()
            .AddModule<SubmodulesModule>()
            .AddModule<GitVersionModule>()
            .AddModule<GitLastCommitModule>()
            .AddModule<FailedModule>();
    })
    .ExecutePipelineAsync();

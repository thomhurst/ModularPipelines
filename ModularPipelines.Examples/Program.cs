using Microsoft.Extensions.Configuration;
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
        options.IgnoreCategories = new[]{ "Ignore" };
    })
    .ConfigureServices((context, collection) =>
    {
        collection.AddModule<SuccessModule>()
            .AddModule<DependentOnSuccessModule>()
            .AddModule<DependentOn2>()
            .AddModule<DependentOn3>()
            .AddModule<DependentOn4>()
            // .AddModule<SuccessModule2>()
            // .AddModule<SuccessModule3>()
            // .AddModule<IgnoredModule>()
            // .AddModule<FailedModule>()
            // .AddModule<GitVersionModule>()
            .AddModule<GitLastCommitModule>()
            .AddModule<DotnetTestModule>();
        // .AddModule<DependentOnSuccessModule>()
        // .AddModule<NugetVersionGeneratorModule>();
        //.AddModule<NotepadPlusPlusInstallerModule>()
        //.AddRequirement<WindowsAdminRequirement>();
    })
    .ExecutePipelineAsync();

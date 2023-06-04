using Microsoft.Extensions.Configuration;
using ModularPipelines.Examples.Modules;
using ModularPipelines.Extensions;
using ModularPipelines.Host;
using ModularPipelines.Requirements;

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
        options.StopOnFirstException = false;
        options.IgnoreCategories = new[] { "Ignore" };
    })
    .ConfigureServices((context, collection) =>
    {
        collection.AddModule<SuccessModule>()
            .AddModule<SuccessModule2>()
            .AddModule<SuccessModule3>()
            .AddModule<IgnoredModule>()
            .AddModule<FailedModule>()
            .AddModule<GitVersionModule>()
            .AddModule<DependentOnSuccessModule>();
        //.AddModule<NotepadPlusPlusInstallerModule>()
        //.AddRequirement<WindowsAdminRequirement>();
    })
    .ExecutePipelineAsync();
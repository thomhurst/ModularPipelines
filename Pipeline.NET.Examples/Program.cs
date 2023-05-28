using Microsoft.Extensions.Configuration;
using Pipeline.NET.Examples;
using Pipeline.NET.Examples.Modules;
using Pipeline.NET.Host;
using Pipeline.NET.Requirements;

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
        options.IgnoreCategories = new[] { "Ignore" };
    })
    .AddModule<SuccessModule>()
    .AddModule<SuccessModule2>()
    .AddModule<SuccessModule3>()
    .AddModule<IgnoredModule>()
    .AddModule<FailedModule>()
    .AddModule<GitVersionModule>()
    .AddModule<KubectlVersionModule>()
    .AddModule<NotepadPlusPlusInstallerModule>()
    .AddRequirement<WindowsAdminRequirement>()
    .ExecutePipelineAsync();
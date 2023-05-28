using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pipeline.NET.Build.Modules;
using Pipeline.NET.Build.Settings;
using Pipeline.NET.Host;

var modules = await PipelineHostBuilder.Create()
    .ConfigureAppConfiguration((context, builder) =>
    {
        builder.AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables();
    })
    .ConfigureServices((context, collection) =>
    {
        collection.Configure<NuGetSettings>(context.Configuration.GetSection("NuGet"));
        collection.Configure<PublishSettings>(context.Configuration.GetSection("Publish"));
    })
    .AddModule<RunUnitTestsModule>()
    .AddModule<PublishPackagesModule>()
    .AddModule<UploadPackagesToNugetModule>()
    .AddModule<CleanModule>()
    .AddModule<BinObjFolderRemovalModule>()
    .ExecutePipelineAsync();
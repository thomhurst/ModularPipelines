using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ModularPipelines.Build;
using ModularPipelines.Build.Modules;
using ModularPipelines.Build.Modules.LocalMachine;
using ModularPipelines.Build.Settings;
using ModularPipelines.Extensions;
using ModularPipelines.Host;
using Octokit;
using Octokit.Internal;

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
        collection.Configure<GitHubSettings>(context.Configuration.GetSection("GitHub"));
        collection.Configure<PublishSettings>(context.Configuration.GetSection("Publish"));

        collection.AddModule<RunUnitTestsModule>()
            .AddModule<NugetVersionGeneratorModule>()
            .AddModule<PackProjectsModule>()
            .AddModule<PackageFilesRemovalModule>()
            .AddModule<PackagePathsParserModule>()
            .AddModule<CodeFormattedNicelyModule>()
            .AddModule<GenerateReadMeModule>()
            .AddPipelineModuleHooks<MyModuleHooks>();

        collection.AddSingleton(sp =>
        {
            var githubSettings = sp.GetRequiredService<IOptions<GitHubSettings>>();
            return new GitHubClient(new ProductHeaderValue("ModularPipelinesBuild"),
                new InMemoryCredentialStore(new Credentials(githubSettings.Value.StandardToken)));
        });

        if (context.HostingEnvironment.IsDevelopment())
        {
            collection.AddModule<CreateLocalNugetFolderModule>()
                .AddModule<AddLocalNugetSourceModule>()
                .AddModule<UploadPackagesToLocalNuGetModule>()
                .AddModule<CheckReleaseNotesAddedModule>();
        }
        else
        {
            collection.AddModule<UploadPackagesToNugetModule>()
                .AddModule<UpdateReleaseNotesModule>();
        }
    })
    .ExecutePipelineAsync();

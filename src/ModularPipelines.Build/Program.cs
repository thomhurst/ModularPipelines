using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Build;
using ModularPipelines.Build.Modules;
using ModularPipelines.Build.Modules.LocalMachine;
using ModularPipelines.Build.Settings;
using ModularPipelines.Extensions;
using ModularPipelines.Host;
using Octokit;
using Octokit.Internal;

await PipelineHostBuilder.Create()
    .ConfigureAppConfiguration((_, builder) =>
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
        collection.Configure<CodacySettings>(context.Configuration.GetSection("Codacy"));
        collection.Configure<CodeCovSettings>(context.Configuration.GetSection("CodeCov"));

        collection
            .AddModule<RunUnitTestsModule>()
            .AddModule<NugetVersionGeneratorModule>()
            .AddModule<FindProjectsModule>()
            .AddModule<FindProjectDependenciesModule>()
            .AddModule<PackProjectsModule>()
            .AddModule<PackageFilesRemovalModule>()
            .AddModule<PackagePathsParserModule>()
            .AddModule<CodeFormattedNicelyModule>()
            .AddModule<GenerateReadMeModule>()
            .AddModule<CodacyCodeCoverageUploader>()
            .AddModule<CodeCovUploaderModule>()
            .AddModule<FormatMarkdownModule>()
            .AddModule<WaitForOtherOperatingSystemBuilds>()
            .AddModule<DownloadCodeCoverageFromOtherOperatingSystemBuildsModule>()
            .AddModule<MergeCoverageModule>()
            .AddModule<ChangedFilesInPullRequestModule>()
            .AddModule<DependabotCommitsModule>()
            .AddModule<PrintEnvironmentVariablesModule>()
            .AddModule<PrintGitInformationModule>()
            .AddPipelineModuleHooks<MyModuleHooks>();

        collection.AddSingleton<IGitHubClient>(sp =>
        {
            var githubSettings = sp.GetRequiredService<IOptions<GitHubSettings>>();
            
            var githubToken = githubSettings.Value.StandardToken;

            if (string.IsNullOrEmpty(githubToken))
            {
                githubToken = "token";
            }
            
            return new GitHubClient(new ProductHeaderValue("ModularPipelinesBuild"),
                new InMemoryCredentialStore(new Credentials(githubToken)));
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
    .ConfigurePipelineOptions((context, options) => options.DefaultRetryCount = 3)
    .SetLogLevel(LogLevel.Debug)
    .ExecutePipelineAsync();
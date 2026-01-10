using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines;
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
        collection.Configure<PipelineSettings>(context.Configuration.GetSection("Pipeline"));
        collection.Configure<NuGetSettings>(context.Configuration.GetSection("NuGet"));
        collection.Configure<LocalNuGetSettings>(context.Configuration.GetSection("LocalNuGet"));
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
            .AddModule<FormatMarkdownModule>()
            .AddModule<ChangedFilesInPullRequestModule>()
            .AddModule<DependabotCommitsModule>()
            .AddModule<PrintEnvironmentVariablesModule>()
            .AddModule<PrintGitInformationModule>()
            .AddModule<PushVersionTagModule>()
            .AddPipelineModuleHooks<MyModuleHooks>();

        collection.AddSingleton<IGitHubClient>(sp =>
        {
            var githubSettings = sp.GetRequiredService<IOptions<GitHubSettings>>();
            var pipelineSettings = sp.GetRequiredService<IOptions<PipelineSettings>>();

            var githubToken = githubSettings.Value.StandardToken;

            if (string.IsNullOrEmpty(githubToken))
            {
                githubToken = "token";
            }

            return new GitHubClient(new ProductHeaderValue(pipelineSettings.Value.GitHubProductHeader),
                new InMemoryCredentialStore(new Credentials(githubToken)));
        });

        // Local NuGet modules should only run in local development, not CI
        // IsDevelopment() returns true for both local dev AND PR builds in CI (DOTNET_ENVIRONMENT=Development)
        // Check common CI environment variables to distinguish local dev from CI
        var isRunningInCI = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI"))
            || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"))
            || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TF_BUILD"))
            || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITLAB_CI"));
        var isLocalDevelopment = context.HostingEnvironment.IsDevelopment() && !isRunningInCI;

        if (isLocalDevelopment)
        {
            collection.AddModule<CreateLocalNugetFolderModule>()
                .AddModule<AddLocalNugetSourceModule>()
                .AddModule<UploadPackagesToLocalNuGetModule>();
        }
        else if (!context.HostingEnvironment.IsDevelopment())
        {
            // Production environment (main branch CI)
            collection.AddModule<UploadPackagesToNugetModule>()
                .AddModule<CreateReleaseModule>();
        }
        // else: CI Development mode (PR builds) - don't register local NuGet or production upload modules
    })
    .ConfigurePipelineOptions((context, options) =>
    {
        var pipelineSettings = context.Configuration.GetSection("Pipeline").Get<PipelineSettings>() ?? new PipelineSettings();
        options.DefaultRetryCount = pipelineSettings.DefaultRetryCount;
    })
    .SetLogLevel(LogLevel.Debug) // Temporarily hardcoded for debugging
    .ExecutePipelineAsync();
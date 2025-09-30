using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Build;
using ModularPipelines.Build.Modules;
using ModularPipelines.Build.Modules.LocalMachine;
using ModularPipelines.Build.Modules.Tests;
using ModularPipelines.Build.Settings;
using ModularPipelines.Distributed.Abstractions;
using ModularPipelines.Distributed.Extensions;
using ModularPipelines.Distributed.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Host;
using Octokit;
using Octokit.Internal;

// Parse command-line arguments to determine execution mode
var commandLineArgs = Environment.GetCommandLineArgs();
var mode = commandLineArgs.Length > 1 ? commandLineArgs[1].ToLowerInvariant() : "local";

var builder = PipelineHostBuilder.Create()
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

        // Register test modules based on execution mode
        if (mode == "orchestrator" || mode == "worker")
        {
            // Distributed execution: register OS-specific test modules
            collection
                .AddModule<RunUnitTestsOnWindowsModule>()
                .AddModule<RunUnitTestsOnLinuxModule>()
                .AddModule<RunUnitTestsOnMacModule>();
        }
        else
        {
            // Local execution: register single test module for current OS
            if (OperatingSystem.IsWindows())
            {
                collection.AddModule<RunUnitTestsOnWindowsModule>();
            }
            else if (OperatingSystem.IsLinux())
            {
                collection.AddModule<RunUnitTestsOnLinuxModule>();
            }
            else if (OperatingSystem.IsMacOS())
            {
                collection.AddModule<RunUnitTestsOnMacModule>();
            }
        }

        collection
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
            .AddModule<MergeCoverageModule>()
            .AddModule<ChangedFilesInPullRequestModule>()
            .AddModule<DependabotCommitsModule>()
            .AddModule<PrintEnvironmentVariablesModule>()
            .AddModule<PrintGitInformationModule>()
            .AddModule<PushVersionTagModule>()
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
                .AddModule<UploadPackagesToLocalNuGetModule>();
        }
        else
        {
            collection.AddModule<UploadPackagesToNugetModule>()
                .AddModule<CreateReleaseModule>();
        }
    })
    .ConfigurePipelineOptions((context, options) => options.DefaultRetryCount = 3)
    .SetLogLevel(Environment.GetEnvironmentVariable("RUNNER_DEBUG") == "1" ? LogLevel.Debug : LogLevel.Information);

// Configure distributed execution if in orchestrator or worker mode
if (mode == "orchestrator")
{
    var port = commandLineArgs.Length > 2 && int.TryParse(commandLineArgs[2], out var p) ? p : 8080;

    Console.WriteLine($"Starting in ORCHESTRATOR mode on port {port}");

    builder.AddDistributedExecution(options =>
    {
        options.Mode = DistributedExecutionMode.Orchestrator;
        options.OrchestratorPort = port;
        options.WorkerHeartbeatTimeout = TimeSpan.FromMinutes(5);
        options.WorkerHeartbeatInterval = TimeSpan.FromSeconds(30);
        options.MaxRetryAttempts = 3;
        options.EnableCompression = true;
    });
}
else if (mode == "worker")
{
    var orchestratorUrl = commandLineArgs.Length > 2 ? commandLineArgs[2] : "http://localhost:8080";
    var workerId = commandLineArgs.Length > 3 ? commandLineArgs[3] : null;

    Console.WriteLine($"Starting in WORKER mode");
    Console.WriteLine($"  Orchestrator URL: {orchestratorUrl}");
    Console.WriteLine($"  Worker ID: {workerId ?? "(auto-generated)"}");

    builder.AddDistributedExecution(options =>
    {
        options.Mode = DistributedExecutionMode.Worker;
        options.OrchestratorUrl = orchestratorUrl;
        // WorkerCapabilities auto-detects OS via DetectCurrentOs()

        if (!string.IsNullOrWhiteSpace(workerId))
        {
            options.WorkerId = workerId;
        }
    });
}
else
{
    Console.WriteLine("Starting in LOCAL mode");
}

await builder.ExecutePipelineAsync();
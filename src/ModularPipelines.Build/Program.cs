using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines;
using ModularPipelines.Build;
using ModularPipelines.Build.Modules;
using ModularPipelines.Build.Modules.LocalMachine;
using ModularPipelines.Build.Modules.UnitTests;
using ModularPipelines.Build.Settings;
using ModularPipelines.Distributed.Artifacts.S3.Extensions;
using ModularPipelines.Distributed.Discovery.Redis;
using ModularPipelines.Distributed.Extensions;
using ModularPipelines.Distributed.SignalR.Extensions;
using ModularPipelines.Extensions;
using Octokit;
using Octokit.Internal;

var builder = Pipeline.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

builder.Services.Configure<PipelineSettings>(builder.Configuration.GetSection("Pipeline"));
builder.Services.Configure<NuGetSettings>(builder.Configuration.GetSection("NuGet"));
builder.Services.Configure<LocalNuGetSettings>(builder.Configuration.GetSection("LocalNuGet"));
builder.Services.Configure<GitHubSettings>(builder.Configuration.GetSection("GitHub"));
builder.Services.Configure<PublishSettings>(builder.Configuration.GetSection("Publish"));
builder.Services.Configure<CodacySettings>(builder.Configuration.GetSection("Codacy"));
builder.Services.Configure<CodeCovSettings>(builder.Configuration.GetSection("CodeCov"));

builder.Services
    .AddModule<BuildSolutionsModule>()
    .AddModule<RunCoreUnitTestsModule>()
    .AddModule<RunAzureUnitTestsModule>()
    .AddModule<RunAnalyzersUnitTestsModule>()
    .AddModule<RunDistributedUnitTestsModule>()
    .AddModule<RunDistributedRedisUnitTestsModule>()
    .AddModule<RunDistributedArtifactsS3UnitTestsModule>()
    .AddModule<RunDistributedSignalRUnitTestsModule>()
    .AddModule<RunDistributedDiscoveryRedisUnitTestsModule>()
    .AddModule<RunAllUnitTestsModule>()
    .AddModule<NugetVersionGeneratorModule>()
    .AddModule<FindProjectsModule>()
    .AddModule<FindProjectDependenciesModule>()
    .AddModule<PackProjectsModule>()
    .AddModule<PackageFilesRemovalModule>()
    .AddModule<PackagePathsParserModule>()
    .AddModule<GenerateReadMeModule>()
    .AddModule<FormatMarkdownModule>()
    .AddModule<PrintEnvironmentVariablesModule>()
    .AddModule<PrintGitInformationModule>()
    .AddModule<PushVersionTagModule>()
    .AddPipelineModuleHooks<MyModuleHooks>();

await BuildPipelineConfiguration.ConfigureDistributedModeAsync(builder);

builder.Services.AddSingleton<IGitHubClient>(sp =>
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
var isLocalDevelopment = builder.Environment.IsDevelopment() && !isRunningInCI;

if (isLocalDevelopment)
{
    builder.Services.AddModule<CreateLocalNugetFolderModule>()
        .AddModule<AddLocalNugetSourceModule>()
        .AddModule<UploadPackagesToLocalNuGetModule>();
}
else if (!builder.Environment.IsDevelopment())
{
    // Production environment (main branch CI)
    builder.Services.AddModule<UploadPackagesToNugetModule>()
        .AddModule<CreateReleaseModule>();
}

// else: CI Development mode (PR builds) - don't register local NuGet or production upload modules
var pipelineSettings = builder.Configuration.GetSection("Pipeline").Get<PipelineSettings>() ?? new PipelineSettings();
builder.Options.DefaultRetryCount = pipelineSettings.DefaultRetryCount;

// Support running only specific categories via environment variable
var runCategories = Environment.GetEnvironmentVariable("Pipeline__RunCategories");
if (!string.IsNullOrEmpty(runCategories))
{
    builder.RunCategories(runCategories.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
}

builder.SetLogLevel(LogLevel.Debug); // Temporarily hardcoded for debugging

var pipeline = builder.Build();
await pipeline.RunAsync();

file static class BuildPipelineConfiguration
{
    public static async Task ConfigureDistributedModeAsync(PipelineBuilder builder)
    {
        var redisRestUrl = Environment.GetEnvironmentVariable("UPSTASH_REDIS_REST_URL");
        var redisRestToken = Environment.GetEnvironmentVariable("UPSTASH_REDIS_REST_TOKEN");
        var instanceIndex = int.TryParse(Environment.GetEnvironmentVariable("INSTANCE_INDEX"), out var idx) ? idx : 0;
        var totalInstances = int.TryParse(Environment.GetEnvironmentVariable("TOTAL_INSTANCES"), out var total) ? total : 1;

        if (string.IsNullOrEmpty(redisRestUrl) || string.IsNullOrEmpty(redisRestToken) || totalInstances <= 1)
        {
            return;
        }

        if (!await IsRedisDiscoveryAvailableAsync(redisRestUrl, redisRestToken))
        {
            return;
        }

        builder.AddDistributedMode(o =>
        {
            o.InstanceIndex = instanceIndex;
            o.TotalInstances = totalInstances;
        });
        builder.AddSignalRDistributedCoordinator(o =>
        {
            o.MaximumReceiveMessageSize = 64 * 1024 * 1024;
            o.EnableTunnel = true;
        });
        builder.AddRedisSignalRDiscovery(o =>
        {
            o.RestUrl = redisRestUrl;
            o.RestToken = redisRestToken;
            o.RunIdentifier = Environment.GetEnvironmentVariable("GITHUB_RUN_ID");
        });

        ConfigureArtifactStore(builder);
    }

    private static async Task<bool> IsRedisDiscoveryAvailableAsync(string restUrl, string restToken)
    {
        try
        {
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
            client.DefaultRequestHeaders.Authorization = new("Bearer", restToken);
            using var response = await client.GetAsync($"{restUrl.TrimEnd('/')}/ping");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            System.Diagnostics.Trace.TraceWarning(
                $"Redis discovery preflight returned HTTP {(int) response.StatusCode}; distributed mode disabled.");
        }
        catch (Exception exception)
        {
            System.Diagnostics.Trace.TraceWarning(
                $"Redis discovery preflight failed ({exception.GetType().Name}); distributed mode disabled.");
        }

        return false;
    }

    private static void ConfigureArtifactStore(PipelineBuilder builder)
    {
        var endpointUrl = Environment.GetEnvironmentVariable("R2_ENDPOINT_URL");
        var accessKey = Environment.GetEnvironmentVariable("R2_ACCESS_KEY");
        var secretKey = Environment.GetEnvironmentVariable("R2_SECRET_KEY");
        if (string.IsNullOrEmpty(endpointUrl) || string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
        {
            return;
        }

        builder.AddS3DistributedArtifactStore(o =>
        {
            o.BucketName = "modular-pipelines";
            o.ServiceUrl = endpointUrl;
            o.AccessKey = accessKey;
            o.SecretKey = secretKey;
            o.ForcePathStyle = true;
        });
    }
}

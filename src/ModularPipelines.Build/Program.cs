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
    .AddModule<RunUnitTestsModule>()
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
    var categories = runCategories.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    builder.RunCategories(categories);
}

builder.SetLogLevel(LogLevel.Debug); // Temporarily hardcoded for debugging

var pipeline = builder.Build();
await pipeline.RunAsync();

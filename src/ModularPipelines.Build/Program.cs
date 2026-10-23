using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines;
using ModularPipelines.Build;
using ModularPipelines.Build.Helpers;
using ModularPipelines.Build.Modules;
using ModularPipelines.Build.Modules.LocalMachine;
using ModularPipelines.Build.Modules.UnitTests;
using ModularPipelines.Build.Settings;
using ModularPipelines.Extensions;

if (args is ["--redis-info"])
{
    using var loggerFactory = LoggerFactory.Create(logging => logging.AddSimpleConsole());
    await DistributedBuildConfiguration.PrintRedisDiagnosticsAsync(Environment.GetEnvironmentVariable, loggerFactory.CreateLogger("RedisDiagnostics")).ConfigureAwait(false);
    return;
}

var versionOutput = new PackageVersionOutput(args);

var builder = Pipeline.CreateBuilder(new PipelineBuilderSettings
{
    Args = versionOutput.PipelineArguments,
    LoadModularPipelinesAssemblies = true,
});

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

builder.Services.Configure<PipelineSettings>(builder.Configuration.GetSection("Pipeline"));
builder.Services.AddSingleton<BuildOutputSharing>();
if (Environment.GetEnvironmentVariable("MODULARPIPELINES_GITHUB_ARTIFACTS") == "true")
{
    builder.Services.AddSingleton<IBuildOutputTransport, GitHubBuildOutputTransport>();
}
builder.Services.Configure<NuGetSettings>(builder.Configuration.GetSection("NuGet"));
builder.Services.Configure<LocalNuGetSettings>(builder.Configuration.GetSection("LocalNuGet"));
builder.Services.Configure<GitHubSettings>(builder.Configuration.GetSection("GitHub"));
builder.Services.Configure<PublishSettings>(builder.Configuration.GetSection("Publish"));
builder.Services.Configure<CodacySettings>(builder.Configuration.GetSection("Codacy"));
builder.Services.Configure<CodeCovSettings>(builder.Configuration.GetSection("CodeCov"));

// Resolve exactly the same version before the native CI build without running release modules.
if (await versionOutput.TryWriteAsync(builder).ConfigureAwait(false))
{
    return;
}

builder
    .AddModule<BuildSolutionsModule>()
    .AddModule<BuildAndTestCoreOnWindowsModule>()
    .AddModule<BuildAndTestCoreOnMacOSModule>()
    .AddModules(UnitTestProjects.ModuleTypes)
    .AddModule<RunAllUnitTestsModule>()
    .AddModule<NugetVersionGeneratorModule>()
    .AddModule<FindProjectsModule>()
    .AddModule<FindProjectDependenciesModule>()
    .AddModule<PackProjectsModule>()
    .AddModule<PackageFilesRemovalModule>()
    .AddModule<PackagePathsParserModule>()
    .AddModule<GenerateReadMeModule>()
    .AddModule<PrintGitInformationModule>()
    .AddModule<PushVersionTagModule>();

// The fast-fail job already ran the OptionsGenerator tests. Registering the module would
// either rerun them or report a pass it never produced; skipping it would cascade through
// RunAllUnitTestsModule, which requires every unit test module.
if (!FastFailValidation.IsComplete(Environment.GetEnvironmentVariable))
{
    builder.AddModule<RunOptionsGeneratorUnitTestsModule>();
}

if (!DistributedBuildConfiguration.Configure(builder, Environment.GetEnvironmentVariable))
{
    return;
}

BuildPipelineConfiguration.ConfigureEnvironmentSpecificModules(builder);

var pipelineSettings = builder.Configuration.GetSection("Pipeline").Get<PipelineSettings>() ?? new PipelineSettings();
builder.ConfigureOptions(options => options with
{
    DefaultRetryCount = pipelineSettings.DefaultRetryCount,
});

// Support running only specific categories via environment variable
var runOnlyCategories = Environment.GetEnvironmentVariable("Pipeline__RunOnlyCategories");
if (!string.IsNullOrEmpty(runOnlyCategories))
{
    builder.ConfigureOptions(options => options with
    {
        RunOnlyCategories = runOnlyCategories.Split(
            ',',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
    });
}

await using var pipeline = await builder.BuildAsync();
await pipeline.RunAsync();

file static class BuildPipelineConfiguration
{
    public static void ConfigureEnvironmentSpecificModules(PipelineBuilder builder)
    {
        // IsDevelopment() returns true for both local development and pull request builds.
        var isRunningInCI = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI"))
            || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"))
            || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TF_BUILD"))
            || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITLAB_CI"));

        if (builder.Environment.IsDevelopment() && !isRunningInCI)
        {
            builder.AddModule<CreateLocalNugetFolderModule>()
                .AddModule<AddLocalNugetSourceModule>()
                .AddModule<UploadPackagesToLocalNuGetModule>();
        }
        else if (!builder.Environment.IsDevelopment())
        {
            builder.AddModule<UploadPackagesToNugetModule>()
                .AddModule<CreateReleaseModule>();
        }
    }
}

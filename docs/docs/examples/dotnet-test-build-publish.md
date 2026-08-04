---
title: .NET Test, Pack & Publish
---

# .NET Test, Pack & Publish

This example tests every unit-test project, packs the remaining projects with a GitVersion-derived
version, and pushes the resulting NuGet packages. Install the core, .NET, and Git packages first:

```powershell
dotnet add package ModularPipelines
dotnet add package ModularPipelines.DotNet
dotnet add package ModularPipelines.Git
```

Set `NUGET_API_KEY` in the pipeline environment, then use this complete pipeline:

```csharp
using ModularPipelines;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

using var builder = Pipeline.CreateBuilder(args);

builder
    .AddModule<NugetVersionGeneratorModule>()
    .AddModule<RunUnitTestsModule>()
    .AddModule<PackProjectsModule>()
    .AddModule<UploadPackagesToNugetModule>();

await builder.ExecutePipelineAsync();

public class NugetVersionGeneratorModule : Module<string>
{
    protected override async Task<string> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var version = await context.Tools.Git.Versioning.GetGitVersioningInformation();
        return version.FullSemVer
            ?? throw new InvalidOperationException("GitVersion did not return a semantic version.");
    }
}

public class RunUnitTestsModule : Module<CommandResult[]>
{
    protected override async Task<CommandResult[]> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var repository = await context.Tools.Git.Information.GetInfoAsync(cancellationToken)
            ?? throw new InvalidOperationException("Git repository information is unavailable.");
        var testProjects = repository.Root
            .GetFiles(file => file.Name.EndsWith(
                ".UnitTests.csproj",
                StringComparison.OrdinalIgnoreCase))
            .ToList();
        var results = new List<CommandResult>();

        foreach (var testProject in testProjects)
        {
            results.Add(await context.Tools.DotNet.TestAsync(
                new DotNetTestOptions
                {
                    Project = testProject.Path,
                },
                cancellationToken: cancellationToken));
        }

        return results.ToArray();
    }
}

[DependsOn<NugetVersionGeneratorModule>]
[DependsOn<RunUnitTestsModule>]
public class PackProjectsModule : Module<CommandResult[]>
{
    protected override async Task<CommandResult[]> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var packageVersion = await context.GetModule<NugetVersionGeneratorModule>();
        var repository = await context.Tools.Git.Information.GetInfoAsync(cancellationToken)
            ?? throw new InvalidOperationException("Git repository information is unavailable.");
        var projects = repository.Root
            .GetFiles(file => file.Extension == ".csproj"
                              && !file.Name.Contains(
                                  "test",
                                  StringComparison.OrdinalIgnoreCase))
            .ToList();
        var packageDirectory = repository.Root
            .GetFolder("artifacts")
            .GetFolder("packages");
        var results = new List<CommandResult>();

        foreach (var project in projects)
        {
            results.Add(await context.Tools.DotNet.PackAsync(
                new DotNetPackOptions
                {
                    ProjectSolution = project.Path,
                    Output = packageDirectory.Path,
                    IncludeSource = true,
                    Properties =
                    [
                        new KeyValue("PackageVersion", packageVersion.Value),
                        new KeyValue("Version", packageVersion.Value),
                    ],
                },
                cancellationToken: cancellationToken));
        }

        return results.ToArray();
    }
}

[DependsOn<PackProjectsModule>]
public class UploadPackagesToNugetModule : Module<CommandResult[]>
{
    protected override async Task<CommandResult[]> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var apiKey = context.Environment.Variables.GetEnvironmentVariable("NUGET_API_KEY");
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        var repository = await context.Tools.Git.Information.GetInfoAsync(cancellationToken)
            ?? throw new InvalidOperationException("Git repository information is unavailable.");
        var packages = repository.Root
            .GetFolder("artifacts")
            .GetFolder("packages")
            .GetFiles(file => file.Extension == ".nupkg")
            .ToList();
        var results = new List<CommandResult>();

        foreach (var package in packages)
        {
            results.Add(await context.Tools.DotNet.NuGet.PushAsync(
                new DotNetNuGetPushOptions
                {
                    Path = package.Path,
                    Source = "https://api.nuget.org/v3/index.json",
                    ApiKey = apiKey,
                },
                cancellationToken: cancellationToken));
        }

        return results.ToArray();
    }
}
```

---
title: .NET Test, Build & Publish
---

# .NET Test, Build & Publish

Here's an example of publishing a NuGet package. Complete with generating sematic versioning, and running tests.

## Generate Version Number

```csharp
public class NugetVersionGeneratorModule : Module<string>
{
    protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var gitVersionInformation = await context.Git().Versioning.GetGitVersioningInformation();
        return gitVersionInformation.FullSemVer;
    }
}
```

## Pack Projects
```csharp
[DependsOn<NugetVersionGeneratorModule>]
public class PackProjectsModule : Module<CommandResult[]>
{
    protected override async Task<CommandResult[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var packageVersion = await GetModule<NugetVersionGeneratorModule>();

        var projects = context.Git().RootDirectory
            .GetFiles(x =>
                x.Extension == ".csproj" && !x.Name.Contains("test", StringComparison.InvariantCultureIgnoreCase))
            .ToList();
        
        return await projects
            .ToAsyncProcessorBuilder()
            .SelectAsync(async projectFile => await context.DotNet().Pack(new DotNetPackOptions
            {
                TargetPath = projectFile.Path,
                IncludeSource = true,
                Properties = new List<string>
                {
                    $"PackageVersion={packageVersion.Value}",
                    $"Version={packageVersion.Value}",
                },
            }, cancellationToken))
            .ProcessOneAtATime();
    }
}

```

## Run Unit Tests

```csharp
[DependsOn<PackProjectsModule>]
public class RunUnitTestsModule : Module<DotNetTestResult[]>
{
    protected override async Task<DotNetTestResult[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        return await context.Git().RootDirectory
            .GetFiles(file => file.Path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)
                              && file.Path.Contains("UnitTests", StringComparison.OrdinalIgnoreCase))
            .ToAsyncProcessorBuilder()
            .SelectAsync(async unitTestProjectFile => await context.DotNet().Test(new DotNetTestOptions
            {
                TargetPath = unitTestProjectFile.Path,
                Collect = "XPlat Code Coverage",
            }, cancellationToken))
            .ProcessInParallel();
    }
}

```

## Upload Packages

```csharp
[DependsOn<RunUnitTestsModule>]
[DependsOn<PackProjectsModule>]
public class UploadPackagesToNugetModule : Module<CommandResult[]>
{
    private readonly IOptions<NuGetSettings> _nugetSettings;

    public UploadPackagesToNugetModule(IOptions<NuGetSettings> nugetSettings, IOptions<PublishSettings> publishSettings)
    {
        _nugetSettings = nugetSettings;
    }

    protected override async Task<CommandResult[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_nugetSettings.Value.ApiKey);

        var packages = context.Git().RootDirectory
            .GetFiles(x =>
                x.Extension == ".nupkg")
            .ToList();

        return await context.NuGet()
            .UploadPackages(new NuGetUploadOptions(packages.AsPaths(), new Uri("https://api.nuget.org/v3/index.json"))
            {
                ApiKey = _nugetSettings.Value.ApiKey!,
            });
    }
}
```
# .NET Test, Pack & Publish

This example tests every unit-test project, packs the remaining projects with a GitVersion-derived version, and pushes the resulting NuGet packages. Install the core, .NET, and Git packages first:

```
dotnet add package ModularPipelines

dotnet add package ModularPipelines.DotNet

dotnet add package ModularPipelines.Git
```

Set `NUGET_API_KEY` in the pipeline environment, then use this complete pipeline:

```
using EnumerableAsyncProcessor.Extensions;

using ModularPipelines;

using ModularPipelines.Attributes;

using ModularPipelines.Context;

using ModularPipelines.DotNet.Options;

using ModularPipelines.Extensions;

using ModularPipelines.Models;

using ModularPipelines.Modules;



var builder = Pipeline.CreateBuilder(args);



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

        var version = await context.Tools.Git.Versioning.GetVersioningInformationAsync(cancellationToken);

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

        return await testProjects

            .ToAsyncProcessorBuilder()

            .SelectAsync(testProject => context.Tools.DotNet.TestAsync(

                new DotNetTestOptions

                {

                    Project = testProject.Path,

                    Arguments =

                    [

                        "--coverage",

                        "--coverage-output-format", "cobertura",

                    ],

                },

                cancellationToken: cancellationToken))

            .ProcessInParallel();

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

                              && !file.Name.EndsWith(

                                  ".UnitTests.csproj",

                                  StringComparison.OrdinalIgnoreCase))

            .ToList();

        var packageDirectory = repository.Root

            .GetFolder("artifacts")

            .GetFolder("packages");

        return await projects

            .ToAsyncProcessorBuilder()

            .SelectAsync(project => context.Tools.DotNet.PackAsync(

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

                cancellationToken: cancellationToken))

            .ProcessInParallel();

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

            .GetFiles(file => file.Extension == ".nupkg"

                              && !file.Name.EndsWith(

                                  ".symbols.nupkg",

                                  StringComparison.OrdinalIgnoreCase))

            .ToList();

        return await packages

            .ToAsyncProcessorBuilder()

            .SelectAsync(package => context.Tools.DotNet.NuGet.PushAsync(

                new DotNetNuGetPushOptions

                {

                    Path = package.Path,

                    Source = "https://api.nuget.org/v3/index.json",

                    ApiKey = apiKey,

                },

                cancellationToken: cancellationToken))

            .ProcessOneAtATime();

    }

}
```

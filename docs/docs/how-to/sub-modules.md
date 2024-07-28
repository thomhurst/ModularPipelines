---
title: Sub-Modules
---

# Sub-Modules

## What are they?
Sub-modules are ways to track and organise blocks of execution where it doesn't make sense to refactor into its own module. This might be when you're iterating through data in a loop.

For instance, you have 10 .NET projects to package into NuGet packages.

By looping through them and declaring each one within their own submodule, then we can easily track failures, and the console progress dialog will show time taken for each submodules.

A submodule takes two arguments: A name, and the code it should execute.

If a submodule fails, it'll throw an exception with the name provided, making it easier to track what specifically failed.

In the example below, we use the .csproj filename as the submodule name. So if one of our projects failed to pack, we'd know which one by the exception message.

## Example

```csharp
public class PackProjectsModule : Module<CommandResult[]>
{
    protected override async Task<CommandResult[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var packageVersion = await GetModule<NugetVersionGeneratorModule>();

        var projects = context.Git().RootDirectory
            .GetFiles(x =>
                x.Extension == ".csproj" && !x.Name.Contains("test", StringComparison.InvariantCultureIgnoreCase))
            .ToList();

        return await PackProjects(context, projects, packageVersion.Value, cancellationToken).ToArrayAsync(cancellationToken: cancellationToken);
    }

    private async IAsyncEnumerable<CommandResult> PackProjects(IPipelineContext context, List<File> projects, string? packageVersion, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        foreach (var project in projects)
        {
            yield return await SubModule(project.Name, () => context.DotNet().Pack(new DotNetPackOptions
            {
                TargetPath = project,
                Configuration = Configuration.Release,
                Properties = new List<string>
                {
                    $"PackageVersion={packageVersion}",
                    $"Version={packageVersion}",
                },
            }, cancellationToken));
        }
    }
}
```
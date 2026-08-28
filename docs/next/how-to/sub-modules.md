# Sub-operations

## What are they?[​](#what-are-they "Direct link to What are they?")

Sub-operations track and organise blocks of execution where it doesn't make sense to refactor into a module. This is useful when iterating through data in a loop.

For instance, you have 10 .NET projects to package into NuGet packages.

By declaring each package operation as a sub-operation, you can track failures and see each duration in the console progress display.

A sub-operation takes a name and a token-aware body to execute.

If a sub-operation fails, its original exception propagates while its name remains visible in progress output.

In the example below, the `.csproj` filename identifies each sub-operation.

## Example[​](#example "Direct link to Example")

```
public class PackProjectsModule : Module<CommandResult[]>

{

    protected override async Task<CommandResult[]> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)

    {

        var packageVersion = await context.GetModule<NugetVersionGeneratorModule>();



        var repositoryInfo = await context.Tools.Git.Information.GetInfoAsync(cancellationToken)

            ?? throw new InvalidOperationException("Git repository information is unavailable.");

        var projects = repositoryInfo.Root

            .GetFiles(x =>

                x.Extension == ".csproj" && !x.Name.Contains("test", StringComparison.InvariantCultureIgnoreCase))

            .ToList();



        return await PackProjects(context, projects, packageVersion.Value, cancellationToken).ToArrayAsync(cancellationToken: cancellationToken);

    }



    private async IAsyncEnumerable<CommandResult> PackProjects(IModuleContext context, List<File> projects, string packageVersion, [EnumeratorCancellation] CancellationToken cancellationToken)

    {

        foreach (var project in projects)

        {

            yield return await context.RunSubModuleAsync(project.Name, token => context.Tools.DotNet.PackAsync(new DotNetPackOptions

            {

                TargetPath = project,

                Configuration = Configuration.Release,

                Properties = new List<string>

                {

                    $"PackageVersion={packageVersion}",

                    $"Version={packageVersion}",

                },

            }, token), cancellationToken);

        }

    }

}
```

using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using TomLonghurst.EnumerableAsyncProcessor.Extensions;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[DependsOn<PackageFilesRemovalModule>, DependsOn<CodeFormattedNicelyModule>]
public class PackProjectsModule : Module<CommandResult[]>
{
    protected override async Task<ModuleResult<CommandResult[]>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var packageVersion = await GetModule<NugetVersionGeneratorModule>();

        var projectFiles = context.Environment
            .GitRootDirectory!
            .GetFiles(f => GetProjectsPredicate(f, context));

        return await projectFiles.ToAsyncProcessorBuilder()
            .SelectAsync(async projectFile => 
                await context.DotNet().Pack(new DotNetPackOptions
            {
                TargetPath = projectFile.Path,
                Configuration = Configuration.Release,
                IncludeSource = !projectFile.Path.Contains("Analyzer"),
                Properties = new List<string>
                {
                    $"PackageVersion={packageVersion.Value}",
                    $"Version={packageVersion.Value}"
                }
            }, cancellationToken)).ProcessOneAtATime();
    }

    private bool GetProjectsPredicate(File file, IModuleContext context)
    {
        var path = file.Path;

        if (!path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (path.Contains("Tests", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (path.EndsWith("ModularPipelines.Build.csproj")
            || path.Contains("Example"))
        {
            return false;
        }

        if (path.EndsWith("ModularPipelines.Analyzers.Package.csproj"))
        {
            return true;
        }

        if (path.Contains("ModularPipelines.Analyzers"))
        {
            return false;
        }

        // Not yet ready
        if (path.EndsWith("ModularPipelines.AWS.csproj"))
        {
            return false;
        }

        context.Logger.LogInformation("Found File: {File}", path);

        return true;
    }
}

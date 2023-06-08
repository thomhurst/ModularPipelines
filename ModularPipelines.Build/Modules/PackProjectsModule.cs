using CliWrap.Buffered;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[DependsOn<PackageFilesRemovalModule>]
public class PackProjectsModule : Module<List<BufferedCommandResult>>
{
    protected override async Task<ModuleResult<List<BufferedCommandResult>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var results = new List<BufferedCommandResult>();

        var packageVersion = await GetModule<NugetVersionGeneratorModule>();

        var unitTestProjectFiles = context.Environment
            .GitRootDirectory!
            .GetFiles(f => GetProjectsPredicate(f, context));

        foreach (var unitTestProjectFile in unitTestProjectFiles)
        {
            results.Add(await context.DotNet().Pack(new DotNetOptions
            {
                TargetPath = unitTestProjectFile.Path,
                Configuration = Configuration.Release,
                ExtraArguments = new List<string>
                {
                    $"/p:PackageVersion={packageVersion.Value}",
                    $"/p:Version={packageVersion.Value}"
                }
            }, cancellationToken));
        }

        return results;
    }

    private bool GetProjectsPredicate(File file, IModuleContext context)
    {
        var path = file.Path;
        if (!path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (path.Contains("UnitTests", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (path.EndsWith("ModularPipelines.Build.csproj")
            || path.EndsWith("ModularPipelines.Examples.csproj"))
        {
            return false;
        }
        
        context.Logger.LogInformation("Found File: {File}", path);

        return true;
    }
}
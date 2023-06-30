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

        var projectFiles = context.Environment
            .GitRootDirectory!
            .GetFiles(f => GetProjectsPredicate(f, context));

        foreach (var projectFile in projectFiles)
        {
            await context.DotNet().Build(new DotNetOptions
            {
                TargetPath = projectFile.Path,
                Configuration = Configuration.Release,
                LogOutput = false
            }, cancellationToken);
            
            results.Add(await context.DotNet().Pack(new DotNetOptions
            {
                TargetPath = projectFile.Path,
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
        
        context.Logger.LogInformation("Found File: {File}", path);

        return true;
    }
}
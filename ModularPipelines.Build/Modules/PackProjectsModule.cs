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
public class PackProjectsModule : Module<List<CommandResult>>
{
    protected override async Task<ModuleResult<List<CommandResult>>?> ExecuteAsync( IModuleContext context, CancellationToken cancellationToken )
    {
        var results = new List<CommandResult>();

        var packageVersion = await GetModule<NugetVersionGeneratorModule>();

        var projectFiles = context.Environment
            .GitRootDirectory!
            .GetFiles( f => GetProjectsPredicate( f, context ) );

        foreach (var projectFile in projectFiles)
        {
            await context.DotNet().Build( new DotNetBuildOptions
            {
                TargetPath = projectFile.Path,
                Configuration = Configuration.Release,
                LogOutput = false
            }, cancellationToken );

            results.Add( await context.DotNet().Pack( new DotNetPackOptions
            {
                TargetPath = projectFile.Path,
                Configuration = Configuration.Release,
                Properties = new[]
                {
                    $"PackageVersion={packageVersion.Value}",
                    $"Version={packageVersion.Value}",
                }
            }, cancellationToken ) );
        }

        return results;
    }

    private bool GetProjectsPredicate( File file, IModuleContext context )
    {
        var path = file.Path;

        if (!path.EndsWith( ".csproj", StringComparison.OrdinalIgnoreCase ))
        {
            return false;
        }

        if (path.Contains( "Tests", StringComparison.OrdinalIgnoreCase ))
        {
            return false;
        }

        if (path.EndsWith( "ModularPipelines.Build.csproj" )
            || path.Contains( "Example" ))
        {
            return false;
        }

        if (path.EndsWith( "ModularPipelines.Analyzers.Package.csproj" ))
        {
            return true;
        }

        if (path.Contains( "ModularPipelines.Analyzers" ))
        {
            return false;
        }

        context.Logger.LogInformation( "Found File: {File}", path );

        return true;
    }
}

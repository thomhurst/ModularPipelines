using CliWrap.Buffered;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
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
    private readonly IOptions<PublishSettings> _options;

    public PackProjectsModule(IOptions<PublishSettings> options)
    {
        _options = options;
    }

    protected override async Task<ModuleResult<List<BufferedCommandResult>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var results = new List<BufferedCommandResult>();

        foreach (var unitTestProjectFile in context.Environment
                     .GitRootDirectory!
                     .GetFiles(f => GetProjectsPredicate(f, context)))
        {
            results.Add(await context.DotNet().Pack(new DotNetOptions
            {
                TargetPath = unitTestProjectFile.Path,
                Configuration = Configuration.Release,
                ExtraArguments = new List<string>
                {
                    $"/p:PackageVersion={_options.Value.Version}",
                    $"/p:Version={_options.Value.Version}"
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
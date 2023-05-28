using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Modules;
using ModularPipelines.DotNet.Options;

namespace ModularPipelines.Build.Modules;

[DependsOn<PackageFilesRemovalModule>]
public class PackProjectsModule : MultiDotNetModule
{
    private readonly IOptions<PublishSettings> _options;

    public PackProjectsModule(IModuleContext context, IOptions<PublishSettings> options) : base(context)
    {
        _options = options;
    }

    protected override MultiDotNetModuleOptions Options
    {
        get => new()
            {
                Command = new[] {"pack"},
                WorkingDirectory = Context.Environment.GitRootDirectory!.FullName,
                ProjectsToInclude = GetProjectsPredicate,
                Configuration = Configuration.Release,
                ExtraArguments = new List<string>
                {
                    $"/p:PackageVersion={_options.Value.Version}",
                    $"/p:Version={_options.Value.Version}"
                }
            };
        set { }
    }

    private bool GetProjectsPredicate(string path)
    {
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
        
        Context.Logger.LogInformation("Found File: {File}", path);

        return true;
    }
}
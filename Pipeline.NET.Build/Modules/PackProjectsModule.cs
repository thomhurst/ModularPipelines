using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pipeline.NET.Attributes;
using Pipeline.NET.Build.Settings;
using Pipeline.NET.Context;
using Pipeline.NET.DotNet.Modules;
using Pipeline.NET.DotNet.Options;

namespace Pipeline.NET.Build.Modules;

[DependsOn<PackageFilesRemovalModule>]
public class PackProjectsModule : MultiDotNetModule
{
    private readonly IOptions<PublishSettings> _options;

    public PackProjectsModule(IModuleContext context, IOptions<PublishSettings> options) : base(context)
    {
        _options = options;
    }

    protected override MultiDotNetModuleOptions Options => new()
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

        if (path.EndsWith("Pipeline.NET.Build.csproj")
            || path.EndsWith("Pipeline.NET.Examples.csproj"))
        {
            return false;
        }
        
        Context.Logger.LogInformation("Found File: {File}", path);

        return true;
    }
}
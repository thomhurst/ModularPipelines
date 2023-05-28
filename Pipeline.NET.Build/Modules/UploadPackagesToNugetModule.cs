using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pipeline.NET.Attributes;
using Pipeline.NET.Build.Settings;
using Pipeline.NET.Context;
using Pipeline.NET.NuGet;

namespace Pipeline.NET.Build.Modules;

[DependsOn<RunUnitTestsModule>]
[DependsOn<PublishPackagesModule>]
public class UploadPackagesToNugetModule : NuGetUploadModule
{
    private readonly IOptions<NuGetSettings> _options;

    public UploadPackagesToNugetModule(IModuleContext context, IOptions<NuGetSettings> options) : base(context)
    {
        ArgumentNullException.ThrowIfNull(options.Value.AccessToken);
        
        _options = options;
    }

    protected override Task InitialiseAsync()
    {
        foreach (var packagePath in PackagePaths)
        {
            Context.Logger.LogInformation("Uploading {File}", packagePath);
        }
        
        return base.InitialiseAsync();
    }

    protected override IEnumerable<string> PackagePaths => Context.FileSystem.GetFiles(
            Context.Environment.GitRootDirectory!.FullName,
            SearchOption.AllDirectories,
            file => file.Extension == ".nupkg"
        ).Select(file => file.FullName)
        .ToList();
    
    protected override string AccessToken
    {
        get
        {
            ArgumentNullException.ThrowIfNull(_options.Value.AccessToken);
            return _options.Value.AccessToken!;
        }
    }

    protected override Uri FeedUri { get; } = new Uri("https://api.nuget.org/v3/index.json");
}
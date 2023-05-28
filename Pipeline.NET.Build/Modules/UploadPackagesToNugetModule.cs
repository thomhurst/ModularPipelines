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
    public UploadPackagesToNugetModule(IModuleContext context, IOptions<NugetSettings> options) : base(context)
    {
        ArgumentNullException.ThrowIfNull(options.Value.ApiKey);
        
        PackagePaths = Context.FileSystem.GetFiles(
            Context.Environment.GitRootDirectory!.FullName,
            SearchOption.AllDirectories,
            file => file.Extension == ".nupkg"
            ).Select(file => file.FullName)
            .ToList();
        
        foreach (var packagePath in PackagePaths)
        {
            Context.Logger.LogInformation("Uploading {File}", packagePath);
        }

        FeedUri = new Uri("https://api.nuget.org/v3/index.json");

        AccessToken = options.Value.ApiKey;
    }

    protected override IEnumerable<string> PackagePaths { get; }
    protected override string AccessToken { get; }
    protected override Uri FeedUri { get; }
}
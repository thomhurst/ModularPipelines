using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.NuGet;

namespace ModularPipelines.Build.Modules.LocalMachine;

[DependsOn<AddLocalNugetSourceModule>]
public class UploadPackagesToLocalNuGetModule : NuGetUploadModule
{
    private readonly IOptions<NuGetSettings> _options;

    public UploadPackagesToLocalNuGetModule(IModuleContext context, IOptions<NuGetSettings> options) : base(context)
    {
        ArgumentNullException.ThrowIfNull(options.Value.ApiKey);
        _options = options;
    }

    protected override async Task InitialiseAsync()
    {
        PackagePaths = (await GetModule<PackagePathsParserModule>()).Value!;
        
        foreach (var packagePath in PackagePaths)
        {
            Context.Logger.LogInformation("Uploading {File}", packagePath);
        }
        
        FeedUri = new Uri((await GetModule<CreateLocalNugetFolderModule>()).Value!);
        
        await base.InitialiseAsync();
    }

    protected override IEnumerable<string> PackagePaths { get; set; } = null!;

    protected override string ApiKey
    {
        get
        {
            ArgumentNullException.ThrowIfNull(_options.Value.ApiKey);
            return _options.Value.ApiKey!;
        }
        set
        {
        }
    }

    protected override Uri FeedUri { get; set; } = null!;
}
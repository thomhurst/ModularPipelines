using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules.LocalMachine;

public class CreateLocalNugetFolderModule : Module<Folder>
{
    private readonly IOptions<LocalNuGetSettings> _localNuGetSettings;

    public CreateLocalNugetFolderModule(IOptions<LocalNuGetSettings> localNuGetSettings)
    {
        _localNuGetSettings = localNuGetSettings;
    }

    protected override Task<Folder?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var settings = _localNuGetSettings.Value;
        var localNugetRepositoryFolder = context.Files.GetFolder(Environment.SpecialFolder.ApplicationData)
            .GetFolder(settings.AppDataFolderName)
            .GetFolder(settings.LocalNugetFolderName)
            .Create();

        context.Logger.LogInformation("Local NuGet Repository Path: {Path}", localNugetRepositoryFolder.Path);

        return Task.FromResult<Folder?>(localNugetRepositoryFolder);
    }
}
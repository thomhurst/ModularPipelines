using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules.LocalMachine;

public class CreateLocalNugetFolderModule : Module<Folder>
{
    public override Task<Folder?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var localNugetRepositoryFolder = context.FileSystem.GetFolder(Environment.SpecialFolder.ApplicationData)
            .GetFolder("ModularPipelines")
            .GetFolder("LocalNuget")
            .Create();

        context.Logger.LogInformation("Local NuGet Repository Path: {Path}", localNugetRepositoryFolder.Path);

        return Task.FromResult<Folder?>(localNugetRepositoryFolder);
    }
}
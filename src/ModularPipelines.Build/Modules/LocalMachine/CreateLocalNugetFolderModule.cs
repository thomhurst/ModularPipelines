using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules.LocalMachine;

public class CreateLocalNugetFolderModule : IModule<Folder>
{
    public async Task<Folder?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var localNugetRepositoryFolder = context.FileSystem.GetFolder(Environment.SpecialFolder.ApplicationData)
            .GetFolder("ModularPipelines")
            .GetFolder("LocalNuget")
            .Create();

        await Task.Yield();

        context.Logger.LogInformation("Local NuGet Repository Path: {Path}", localNugetRepositoryFolder.Path);

        return localNugetRepositoryFolder;
    }
}
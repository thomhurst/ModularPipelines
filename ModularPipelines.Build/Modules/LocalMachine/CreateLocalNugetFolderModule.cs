using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules.LocalMachine;

[DependsOn<RunUnitTestsModule>]
[DependsOn<PackagePathsParserModule>]
public class CreateLocalNugetFolderModule : Module<string>
{
    protected override async Task<ModuleResult<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var userAppData = context.FileSystem.GetFolder(Environment.SpecialFolder.ApplicationData).Path;

        var localNugetRepositoryFolderPath = Path.Combine(userAppData, "ModularPipelines", "LocalNuget");

        if (!Directory.Exists(localNugetRepositoryFolderPath))
        {
            Directory.CreateDirectory(localNugetRepositoryFolderPath);
        }

        await Task.Yield();
        
        context.Logger.LogInformation("Local NuGet Repository Path: {Path}", localNugetRepositoryFolderPath);

        return localNugetRepositoryFolderPath;
    }
}
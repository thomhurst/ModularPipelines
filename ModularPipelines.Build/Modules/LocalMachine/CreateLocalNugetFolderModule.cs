using System.Reflection;
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
    public CreateLocalNugetFolderModule(IModuleContext context) : base(context)
    {
    }

    protected override async Task<ModuleResult<string>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var userAppData = Context.FileSystem.GetFolder(Environment.SpecialFolder.ApplicationData).FullName;

        var localNugetRepositoryFolderPath = Path.Combine(userAppData, "ModularPipelines", "LocalNuget");

        if (!Directory.Exists(localNugetRepositoryFolderPath))
        {
            Directory.CreateDirectory(localNugetRepositoryFolderPath);
        }

        await Task.Yield();
        
        Context.Logger.LogInformation("Local NuGet Repository Path: {Path}", localNugetRepositoryFolderPath);

        return localNugetRepositoryFolderPath;
    }
}
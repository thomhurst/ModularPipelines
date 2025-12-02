using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.FileSystem;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;

namespace ModularPipelines.Build.Modules.LocalMachine;

[DependsOn<CreateLocalNugetFolderModule>]
public class AddLocalNugetSourceModule : Module<CommandResult>, IIgnoreFailures
{
    public Task<bool> ShouldIgnoreFailures(IPipelineContext context, Exception exception)
    {
        return Task.FromResult(exception is CommandException commandException &&
                               commandException.StandardOutput.Contains("The name specified has already been added to the list of available package sources"));
    }

    public override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var localNugetPathResult = context.GetModule<CreateLocalNugetFolderModule, Folder>();

        return await context.DotNet().Nuget.Add.Source(new DotNetNugetAddSourceOptions(packageSourcePath: localNugetPathResult.Value.AssertExists())
        {
            Name = "ModularPipelinesLocalNuGet",
        }, cancellationToken);
    }
}
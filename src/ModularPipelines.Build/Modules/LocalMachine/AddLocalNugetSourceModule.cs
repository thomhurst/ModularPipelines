using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules.LocalMachine;

[DependsOn<CreateLocalNugetFolderModule>]
public class AddLocalNugetSourceModule : Module<CommandResult>
{
    /// <inheritdoc/>
    protected override Task<bool> ShouldIgnoreFailures(IPipelineContext context, Exception exception)
    {
        return Task.FromResult(exception is CommandException commandException &&
                               commandException.StandardOutput.Contains("The name specified has already been added to the list of available package sources"));
    }

    /// <inheritdoc/>
    protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var localNugetPathResult = await GetModule<CreateLocalNugetFolderModule>();

        return await context.DotNet().Nuget.Add.Source(new DotNetNugetAddSourceOptions(packageSourcePath: localNugetPathResult.Value.AssertExists())
        {
            Name = "ModularPipelinesLocalNuGet",
        }, cancellationToken);
    }
}
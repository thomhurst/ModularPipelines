using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet")]
public class GcloudContainerFleetMultiClusterServices
{
    public GcloudContainerFleetMultiClusterServices(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Describe(GcloudContainerFleetMultiClusterServicesDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetMultiClusterServicesDescribeOptions(), token);
    }

    public async Task<CommandResult> Disable(GcloudContainerFleetMultiClusterServicesDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetMultiClusterServicesDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(GcloudContainerFleetMultiClusterServicesEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetMultiClusterServicesEnableOptions(), token);
    }
}
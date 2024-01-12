using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet")]
public class GcloudContainerFleetFleetobservability
{
    public GcloudContainerFleetFleetobservability(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Describe(GcloudContainerFleetFleetobservabilityDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetFleetobservabilityDescribeOptions(), token);
    }

    public async Task<CommandResult> Disable(GcloudContainerFleetFleetobservabilityDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetFleetobservabilityDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(GcloudContainerFleetFleetobservabilityEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetFleetobservabilityEnableOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudContainerFleetFleetobservabilityUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetFleetobservabilityUpdateOptions(), token);
    }
}
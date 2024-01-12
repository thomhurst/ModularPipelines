using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub")]
public class GcloudContainerHubFleetobservability
{
    public GcloudContainerHubFleetobservability(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Describe(GcloudContainerHubFleetobservabilityDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubFleetobservabilityDescribeOptions(), token);
    }

    public async Task<CommandResult> Disable(GcloudContainerHubFleetobservabilityDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubFleetobservabilityDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(GcloudContainerHubFleetobservabilityEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubFleetobservabilityEnableOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudContainerHubFleetobservabilityUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubFleetobservabilityUpdateOptions(), token);
    }
}
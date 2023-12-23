using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsRoute53RecoveryCluster
{
    public AwsRoute53RecoveryCluster(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> GetRoutingControlState(AwsRoute53RecoveryClusterGetRoutingControlStateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRoutingControls(AwsRoute53RecoveryClusterListRoutingControlsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53RecoveryClusterListRoutingControlsOptions(), token);
    }

    public async Task<CommandResult> UpdateRoutingControlState(AwsRoute53RecoveryClusterUpdateRoutingControlStateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRoutingControlStates(AwsRoute53RecoveryClusterUpdateRoutingControlStatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
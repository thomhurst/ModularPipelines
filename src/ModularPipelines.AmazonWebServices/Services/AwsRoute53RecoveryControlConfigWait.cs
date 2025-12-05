using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-control-config")]
public class AwsRoute53RecoveryControlConfigWait
{
    public AwsRoute53RecoveryControlConfigWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> ClusterCreated(AwsRoute53RecoveryControlConfigWaitClusterCreatedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ClusterDeleted(AwsRoute53RecoveryControlConfigWaitClusterDeletedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ControlPanelCreated(AwsRoute53RecoveryControlConfigWaitControlPanelCreatedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ControlPanelDeleted(AwsRoute53RecoveryControlConfigWaitControlPanelDeletedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RoutingControlCreated(AwsRoute53RecoveryControlConfigWaitRoutingControlCreatedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RoutingControlDeleted(AwsRoute53RecoveryControlConfigWaitRoutingControlDeletedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
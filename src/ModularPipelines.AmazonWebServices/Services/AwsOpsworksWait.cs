using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("opsworks")]
public class AwsOpsworksWait
{
    public AwsOpsworksWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AppExists(AwsOpsworksWaitAppExistsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksWaitAppExistsOptions(), token);
    }

    public async Task<CommandResult> DeploymentSuccessful(AwsOpsworksWaitDeploymentSuccessfulOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksWaitDeploymentSuccessfulOptions(), token);
    }

    public async Task<CommandResult> InstanceOnline(AwsOpsworksWaitInstanceOnlineOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksWaitInstanceOnlineOptions(), token);
    }

    public async Task<CommandResult> InstanceRegistered(AwsOpsworksWaitInstanceRegisteredOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksWaitInstanceRegisteredOptions(), token);
    }

    public async Task<CommandResult> InstanceStopped(AwsOpsworksWaitInstanceStoppedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksWaitInstanceStoppedOptions(), token);
    }

    public async Task<CommandResult> InstanceTerminated(AwsOpsworksWaitInstanceTerminatedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksWaitInstanceTerminatedOptions(), token);
    }
}
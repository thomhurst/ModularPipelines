using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> AppExists(AwsOpsworksWaitAppExistsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksWaitAppExistsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeploymentSuccessful(AwsOpsworksWaitDeploymentSuccessfulOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksWaitDeploymentSuccessfulOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> InstanceOnline(AwsOpsworksWaitInstanceOnlineOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksWaitInstanceOnlineOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> InstanceRegistered(AwsOpsworksWaitInstanceRegisteredOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksWaitInstanceRegisteredOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> InstanceStopped(AwsOpsworksWaitInstanceStoppedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksWaitInstanceStoppedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> InstanceTerminated(AwsOpsworksWaitInstanceTerminatedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksWaitInstanceTerminatedOptions(), executionOptions, cancellationToken);
    }
}
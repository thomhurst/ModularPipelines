using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("ec2")]
public class AwsEc2Wait
{
    public AwsEc2Wait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> BundleTaskComplete(AwsEc2WaitBundleTaskCompleteOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitBundleTaskCompleteOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ConversionTaskCancelled(AwsEc2WaitConversionTaskCancelledOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitConversionTaskCancelledOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ConversionTaskCompleted(AwsEc2WaitConversionTaskCompletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitConversionTaskCompletedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ConversionTaskDeleted(AwsEc2WaitConversionTaskDeletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitConversionTaskDeletedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CustomerGatewayAvailable(AwsEc2WaitCustomerGatewayAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitCustomerGatewayAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExportTaskCancelled(AwsEc2WaitExportTaskCancelledOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitExportTaskCancelledOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExportTaskCompleted(AwsEc2WaitExportTaskCompletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitExportTaskCompletedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ImageAvailable(AwsEc2WaitImageAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitImageAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ImageExists(AwsEc2WaitImageExistsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitImageExistsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> InstanceExists(AwsEc2WaitInstanceExistsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitInstanceExistsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> InstanceRunning(AwsEc2WaitInstanceRunningOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitInstanceRunningOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> InstanceStatusOk(AwsEc2WaitInstanceStatusOkOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitInstanceStatusOkOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> InstanceStopped(AwsEc2WaitInstanceStoppedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitInstanceStoppedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> InstanceTerminated(AwsEc2WaitInstanceTerminatedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitInstanceTerminatedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> InternetGatewayExists(AwsEc2WaitInternetGatewayExistsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitInternetGatewayExistsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> KeyPairExists(AwsEc2WaitKeyPairExistsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitKeyPairExistsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> NatGatewayAvailable(AwsEc2WaitNatGatewayAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitNatGatewayAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> NatGatewayDeleted(AwsEc2WaitNatGatewayDeletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitNatGatewayDeletedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> NetworkInterfaceAvailable(AwsEc2WaitNetworkInterfaceAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitNetworkInterfaceAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PasswordDataAvailable(AwsEc2WaitPasswordDataAvailableOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SecurityGroupExists(AwsEc2WaitSecurityGroupExistsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitSecurityGroupExistsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SnapshotCompleted(AwsEc2WaitSnapshotCompletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitSnapshotCompletedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SnapshotImported(AwsEc2WaitSnapshotImportedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitSnapshotImportedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SpotInstanceRequestFulfilled(AwsEc2WaitSpotInstanceRequestFulfilledOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitSpotInstanceRequestFulfilledOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StoreImageTaskComplete(AwsEc2WaitStoreImageTaskCompleteOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitStoreImageTaskCompleteOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SubnetAvailable(AwsEc2WaitSubnetAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitSubnetAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SystemStatusOk(AwsEc2WaitSystemStatusOkOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitSystemStatusOkOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> VolumeAvailable(AwsEc2WaitVolumeAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitVolumeAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> VolumeDeleted(AwsEc2WaitVolumeDeletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitVolumeDeletedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> VolumeInUse(AwsEc2WaitVolumeInUseOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitVolumeInUseOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> VpcAvailable(AwsEc2WaitVpcAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitVpcAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> VpcExists(AwsEc2WaitVpcExistsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitVpcExistsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> VpcPeeringConnectionDeleted(AwsEc2WaitVpcPeeringConnectionDeletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitVpcPeeringConnectionDeletedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> VpcPeeringConnectionExists(AwsEc2WaitVpcPeeringConnectionExistsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitVpcPeeringConnectionExistsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> VpnConnectionAvailable(AwsEc2WaitVpnConnectionAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitVpnConnectionAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> VpnConnectionDeleted(AwsEc2WaitVpnConnectionDeletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitVpnConnectionDeletedOptions(), executionOptions, cancellationToken);
    }
}
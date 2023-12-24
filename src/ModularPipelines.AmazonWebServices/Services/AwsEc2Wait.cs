using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2")]
public class AwsEc2Wait
{
    public AwsEc2Wait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> BundleTaskComplete(AwsEc2WaitBundleTaskCompleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitBundleTaskCompleteOptions(), token);
    }

    public async Task<CommandResult> ConversionTaskCancelled(AwsEc2WaitConversionTaskCancelledOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitConversionTaskCancelledOptions(), token);
    }

    public async Task<CommandResult> ConversionTaskCompleted(AwsEc2WaitConversionTaskCompletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitConversionTaskCompletedOptions(), token);
    }

    public async Task<CommandResult> ConversionTaskDeleted(AwsEc2WaitConversionTaskDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitConversionTaskDeletedOptions(), token);
    }

    public async Task<CommandResult> CustomerGatewayAvailable(AwsEc2WaitCustomerGatewayAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitCustomerGatewayAvailableOptions(), token);
    }

    public async Task<CommandResult> ExportTaskCancelled(AwsEc2WaitExportTaskCancelledOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitExportTaskCancelledOptions(), token);
    }

    public async Task<CommandResult> ExportTaskCompleted(AwsEc2WaitExportTaskCompletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitExportTaskCompletedOptions(), token);
    }

    public async Task<CommandResult> ImageAvailable(AwsEc2WaitImageAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitImageAvailableOptions(), token);
    }

    public async Task<CommandResult> ImageExists(AwsEc2WaitImageExistsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitImageExistsOptions(), token);
    }

    public async Task<CommandResult> InstanceExists(AwsEc2WaitInstanceExistsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitInstanceExistsOptions(), token);
    }

    public async Task<CommandResult> InstanceRunning(AwsEc2WaitInstanceRunningOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitInstanceRunningOptions(), token);
    }

    public async Task<CommandResult> InstanceStatusOk(AwsEc2WaitInstanceStatusOkOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitInstanceStatusOkOptions(), token);
    }

    public async Task<CommandResult> InstanceStopped(AwsEc2WaitInstanceStoppedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitInstanceStoppedOptions(), token);
    }

    public async Task<CommandResult> InstanceTerminated(AwsEc2WaitInstanceTerminatedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitInstanceTerminatedOptions(), token);
    }

    public async Task<CommandResult> InternetGatewayExists(AwsEc2WaitInternetGatewayExistsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitInternetGatewayExistsOptions(), token);
    }

    public async Task<CommandResult> KeyPairExists(AwsEc2WaitKeyPairExistsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitKeyPairExistsOptions(), token);
    }

    public async Task<CommandResult> NatGatewayAvailable(AwsEc2WaitNatGatewayAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitNatGatewayAvailableOptions(), token);
    }

    public async Task<CommandResult> NatGatewayDeleted(AwsEc2WaitNatGatewayDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitNatGatewayDeletedOptions(), token);
    }

    public async Task<CommandResult> NetworkInterfaceAvailable(AwsEc2WaitNetworkInterfaceAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitNetworkInterfaceAvailableOptions(), token);
    }

    public async Task<CommandResult> PasswordDataAvailable(AwsEc2WaitPasswordDataAvailableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SecurityGroupExists(AwsEc2WaitSecurityGroupExistsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitSecurityGroupExistsOptions(), token);
    }

    public async Task<CommandResult> SnapshotCompleted(AwsEc2WaitSnapshotCompletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitSnapshotCompletedOptions(), token);
    }

    public async Task<CommandResult> SnapshotImported(AwsEc2WaitSnapshotImportedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitSnapshotImportedOptions(), token);
    }

    public async Task<CommandResult> SpotInstanceRequestFulfilled(AwsEc2WaitSpotInstanceRequestFulfilledOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitSpotInstanceRequestFulfilledOptions(), token);
    }

    public async Task<CommandResult> StoreImageTaskComplete(AwsEc2WaitStoreImageTaskCompleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitStoreImageTaskCompleteOptions(), token);
    }

    public async Task<CommandResult> SubnetAvailable(AwsEc2WaitSubnetAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitSubnetAvailableOptions(), token);
    }

    public async Task<CommandResult> SystemStatusOk(AwsEc2WaitSystemStatusOkOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitSystemStatusOkOptions(), token);
    }

    public async Task<CommandResult> VolumeAvailable(AwsEc2WaitVolumeAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitVolumeAvailableOptions(), token);
    }

    public async Task<CommandResult> VolumeDeleted(AwsEc2WaitVolumeDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitVolumeDeletedOptions(), token);
    }

    public async Task<CommandResult> VolumeInUse(AwsEc2WaitVolumeInUseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitVolumeInUseOptions(), token);
    }

    public async Task<CommandResult> VpcAvailable(AwsEc2WaitVpcAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitVpcAvailableOptions(), token);
    }

    public async Task<CommandResult> VpcExists(AwsEc2WaitVpcExistsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitVpcExistsOptions(), token);
    }

    public async Task<CommandResult> VpcPeeringConnectionDeleted(AwsEc2WaitVpcPeeringConnectionDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitVpcPeeringConnectionDeletedOptions(), token);
    }

    public async Task<CommandResult> VpcPeeringConnectionExists(AwsEc2WaitVpcPeeringConnectionExistsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitVpcPeeringConnectionExistsOptions(), token);
    }

    public async Task<CommandResult> VpnConnectionAvailable(AwsEc2WaitVpnConnectionAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitVpnConnectionAvailableOptions(), token);
    }

    public async Task<CommandResult> VpnConnectionDeleted(AwsEc2WaitVpnConnectionDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2WaitVpnConnectionDeletedOptions(), token);
    }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsGreengrassv2
{
    public AwsGreengrassv2(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateServiceRoleToAccount(AwsGreengrassv2AssociateServiceRoleToAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchAssociateClientDeviceWithCoreDevice(AwsGreengrassv2BatchAssociateClientDeviceWithCoreDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDisassociateClientDeviceFromCoreDevice(AwsGreengrassv2BatchDisassociateClientDeviceFromCoreDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelDeployment(AwsGreengrassv2CancelDeploymentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateComponentVersion(AwsGreengrassv2CreateComponentVersionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassv2CreateComponentVersionOptions(), token);
    }

    public async Task<CommandResult> CreateDeployment(AwsGreengrassv2CreateDeploymentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteComponent(AwsGreengrassv2DeleteComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCoreDevice(AwsGreengrassv2DeleteCoreDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDeployment(AwsGreengrassv2DeleteDeploymentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeComponent(AwsGreengrassv2DescribeComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateServiceRoleFromAccount(AwsGreengrassv2DisassociateServiceRoleFromAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassv2DisassociateServiceRoleFromAccountOptions(), token);
    }

    public async Task<CommandResult> GetComponent(AwsGreengrassv2GetComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetComponentVersionArtifact(AwsGreengrassv2GetComponentVersionArtifactOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConnectivityInfo(AwsGreengrassv2GetConnectivityInfoOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCoreDevice(AwsGreengrassv2GetCoreDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDeployment(AwsGreengrassv2GetDeploymentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetServiceRoleForAccount(AwsGreengrassv2GetServiceRoleForAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassv2GetServiceRoleForAccountOptions(), token);
    }

    public async Task<CommandResult> ListClientDevicesAssociatedWithCoreDevice(AwsGreengrassv2ListClientDevicesAssociatedWithCoreDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListComponentVersions(AwsGreengrassv2ListComponentVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListComponents(AwsGreengrassv2ListComponentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassv2ListComponentsOptions(), token);
    }

    public async Task<CommandResult> ListCoreDevices(AwsGreengrassv2ListCoreDevicesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassv2ListCoreDevicesOptions(), token);
    }

    public async Task<CommandResult> ListDeployments(AwsGreengrassv2ListDeploymentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassv2ListDeploymentsOptions(), token);
    }

    public async Task<CommandResult> ListEffectiveDeployments(AwsGreengrassv2ListEffectiveDeploymentsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListInstalledComponents(AwsGreengrassv2ListInstalledComponentsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsGreengrassv2ListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResolveComponentCandidates(AwsGreengrassv2ResolveComponentCandidatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassv2ResolveComponentCandidatesOptions(), token);
    }

    public async Task<CommandResult> TagResource(AwsGreengrassv2TagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsGreengrassv2UntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConnectivityInfo(AwsGreengrassv2UpdateConnectivityInfoOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
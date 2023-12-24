using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsOpsworks
{
    public AwsOpsworks(
        AwsOpsworksWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsOpsworksWait Wait { get; }

    public async Task<CommandResult> AssignInstance(AwsOpsworksAssignInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssignVolume(AwsOpsworksAssignVolumeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateElasticIp(AwsOpsworksAssociateElasticIpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachElasticLoadBalancer(AwsOpsworksAttachElasticLoadBalancerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CloneStack(AwsOpsworksCloneStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateApp(AwsOpsworksCreateAppOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDeployment(AwsOpsworksCreateDeploymentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateInstance(AwsOpsworksCreateInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLayer(AwsOpsworksCreateLayerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateStack(AwsOpsworksCreateStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateUserProfile(AwsOpsworksCreateUserProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteApp(AwsOpsworksDeleteAppOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInstance(AwsOpsworksDeleteInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLayer(AwsOpsworksDeleteLayerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteStack(AwsOpsworksDeleteStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteUserProfile(AwsOpsworksDeleteUserProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterEcsCluster(AwsOpsworksDeregisterEcsClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterElasticIp(AwsOpsworksDeregisterElasticIpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterInstance(AwsOpsworksDeregisterInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterRdsDbInstance(AwsOpsworksDeregisterRdsDbInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterVolume(AwsOpsworksDeregisterVolumeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAgentVersions(AwsOpsworksDescribeAgentVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksDescribeAgentVersionsOptions(), token);
    }

    public async Task<CommandResult> DescribeApps(AwsOpsworksDescribeAppsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksDescribeAppsOptions(), token);
    }

    public async Task<CommandResult> DescribeCommands(AwsOpsworksDescribeCommandsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksDescribeCommandsOptions(), token);
    }

    public async Task<CommandResult> DescribeDeployments(AwsOpsworksDescribeDeploymentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksDescribeDeploymentsOptions(), token);
    }

    public async Task<CommandResult> DescribeEcsClusters(AwsOpsworksDescribeEcsClustersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksDescribeEcsClustersOptions(), token);
    }

    public async Task<CommandResult> DescribeElasticIps(AwsOpsworksDescribeElasticIpsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksDescribeElasticIpsOptions(), token);
    }

    public async Task<CommandResult> DescribeElasticLoadBalancers(AwsOpsworksDescribeElasticLoadBalancersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksDescribeElasticLoadBalancersOptions(), token);
    }

    public async Task<CommandResult> DescribeInstances(AwsOpsworksDescribeInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksDescribeInstancesOptions(), token);
    }

    public async Task<CommandResult> DescribeLayers(AwsOpsworksDescribeLayersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksDescribeLayersOptions(), token);
    }

    public async Task<CommandResult> DescribeLoadBasedAutoScaling(AwsOpsworksDescribeLoadBasedAutoScalingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMyUserProfile(AwsOpsworksDescribeMyUserProfileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksDescribeMyUserProfileOptions(), token);
    }

    public async Task<CommandResult> DescribeOperatingSystems(AwsOpsworksDescribeOperatingSystemsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksDescribeOperatingSystemsOptions(), token);
    }

    public async Task<CommandResult> DescribePermissions(AwsOpsworksDescribePermissionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksDescribePermissionsOptions(), token);
    }

    public async Task<CommandResult> DescribeRaidArrays(AwsOpsworksDescribeRaidArraysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksDescribeRaidArraysOptions(), token);
    }

    public async Task<CommandResult> DescribeRdsDbInstances(AwsOpsworksDescribeRdsDbInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeServiceErrors(AwsOpsworksDescribeServiceErrorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksDescribeServiceErrorsOptions(), token);
    }

    public async Task<CommandResult> DescribeStackProvisioningParameters(AwsOpsworksDescribeStackProvisioningParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeStackSummary(AwsOpsworksDescribeStackSummaryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeStacks(AwsOpsworksDescribeStacksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksDescribeStacksOptions(), token);
    }

    public async Task<CommandResult> DescribeTimeBasedAutoScaling(AwsOpsworksDescribeTimeBasedAutoScalingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeUserProfiles(AwsOpsworksDescribeUserProfilesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksDescribeUserProfilesOptions(), token);
    }

    public async Task<CommandResult> DescribeVolumes(AwsOpsworksDescribeVolumesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksDescribeVolumesOptions(), token);
    }

    public async Task<CommandResult> DetachElasticLoadBalancer(AwsOpsworksDetachElasticLoadBalancerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateElasticIp(AwsOpsworksDisassociateElasticIpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetHostnameSuggestion(AwsOpsworksGetHostnameSuggestionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GrantAccess(AwsOpsworksGrantAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTags(AwsOpsworksListTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RebootInstance(AwsOpsworksRebootInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Register(AwsOpsworksRegisterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterEcsCluster(AwsOpsworksRegisterEcsClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterElasticIp(AwsOpsworksRegisterElasticIpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterInstance(AwsOpsworksRegisterInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterRdsDbInstance(AwsOpsworksRegisterRdsDbInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterVolume(AwsOpsworksRegisterVolumeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetLoadBasedAutoScaling(AwsOpsworksSetLoadBasedAutoScalingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetPermission(AwsOpsworksSetPermissionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetTimeBasedAutoScaling(AwsOpsworksSetTimeBasedAutoScalingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartInstance(AwsOpsworksStartInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartStack(AwsOpsworksStartStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopInstance(AwsOpsworksStopInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopStack(AwsOpsworksStopStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsOpsworksTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UnassignInstance(AwsOpsworksUnassignInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UnassignVolume(AwsOpsworksUnassignVolumeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsOpsworksUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateApp(AwsOpsworksUpdateAppOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateElasticIp(AwsOpsworksUpdateElasticIpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateInstance(AwsOpsworksUpdateInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateLayer(AwsOpsworksUpdateLayerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateMyUserProfile(AwsOpsworksUpdateMyUserProfileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpsworksUpdateMyUserProfileOptions(), token);
    }

    public async Task<CommandResult> UpdateRdsDbInstance(AwsOpsworksUpdateRdsDbInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateStack(AwsOpsworksUpdateStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateUserProfile(AwsOpsworksUpdateUserProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateVolume(AwsOpsworksUpdateVolumeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
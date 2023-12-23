using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsElb
{
    public AwsElb(
        AwsElbWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsElbWait Wait { get; }

    public async Task<CommandResult> AddTags(AwsElbAddTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ApplySecurityGroupsToLoadBalancer(AwsElbApplySecurityGroupsToLoadBalancerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachLoadBalancerToSubnets(AwsElbAttachLoadBalancerToSubnetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfigureHealthCheck(AwsElbConfigureHealthCheckOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAppCookieStickinessPolicy(AwsElbCreateAppCookieStickinessPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLbCookieStickinessPolicy(AwsElbCreateLbCookieStickinessPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLoadBalancer(AwsElbCreateLoadBalancerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLoadBalancerListeners(AwsElbCreateLoadBalancerListenersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLoadBalancerPolicy(AwsElbCreateLoadBalancerPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLoadBalancer(AwsElbDeleteLoadBalancerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLoadBalancerListeners(AwsElbDeleteLoadBalancerListenersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLoadBalancerPolicy(AwsElbDeleteLoadBalancerPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterInstancesFromLoadBalancer(AwsElbDeregisterInstancesFromLoadBalancerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAccountLimits(AwsElbDescribeAccountLimitsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElbDescribeAccountLimitsOptions(), token);
    }

    public async Task<CommandResult> DescribeInstanceHealth(AwsElbDescribeInstanceHealthOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeLoadBalancerAttributes(AwsElbDescribeLoadBalancerAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeLoadBalancerPolicies(AwsElbDescribeLoadBalancerPoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElbDescribeLoadBalancerPoliciesOptions(), token);
    }

    public async Task<CommandResult> DescribeLoadBalancerPolicyTypes(AwsElbDescribeLoadBalancerPolicyTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElbDescribeLoadBalancerPolicyTypesOptions(), token);
    }

    public async Task<CommandResult> DescribeLoadBalancers(AwsElbDescribeLoadBalancersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElbDescribeLoadBalancersOptions(), token);
    }

    public async Task<CommandResult> DescribeTags(AwsElbDescribeTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachLoadBalancerFromSubnets(AwsElbDetachLoadBalancerFromSubnetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableAvailabilityZonesForLoadBalancer(AwsElbDisableAvailabilityZonesForLoadBalancerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableAvailabilityZonesForLoadBalancer(AwsElbEnableAvailabilityZonesForLoadBalancerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyLoadBalancerAttributes(AwsElbModifyLoadBalancerAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterInstancesWithLoadBalancer(AwsElbRegisterInstancesWithLoadBalancerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveTags(AwsElbRemoveTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetLoadBalancerListenerSslCertificate(AwsElbSetLoadBalancerListenerSslCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetLoadBalancerPoliciesForBackendServer(AwsElbSetLoadBalancerPoliciesForBackendServerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetLoadBalancerPoliciesOfListener(AwsElbSetLoadBalancerPoliciesOfListenerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
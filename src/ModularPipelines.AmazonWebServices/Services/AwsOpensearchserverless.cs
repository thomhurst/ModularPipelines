using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsOpensearchserverless
{
    public AwsOpensearchserverless(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> BatchGetCollection(AwsOpensearchserverlessBatchGetCollectionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpensearchserverlessBatchGetCollectionOptions(), token);
    }

    public async Task<CommandResult> BatchGetEffectiveLifecyclePolicy(AwsOpensearchserverlessBatchGetEffectiveLifecyclePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetLifecyclePolicy(AwsOpensearchserverlessBatchGetLifecyclePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetVpcEndpoint(AwsOpensearchserverlessBatchGetVpcEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAccessPolicy(AwsOpensearchserverlessCreateAccessPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCollection(AwsOpensearchserverlessCreateCollectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLifecyclePolicy(AwsOpensearchserverlessCreateLifecyclePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSecurityConfig(AwsOpensearchserverlessCreateSecurityConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSecurityPolicy(AwsOpensearchserverlessCreateSecurityPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVpcEndpoint(AwsOpensearchserverlessCreateVpcEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAccessPolicy(AwsOpensearchserverlessDeleteAccessPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCollection(AwsOpensearchserverlessDeleteCollectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLifecyclePolicy(AwsOpensearchserverlessDeleteLifecyclePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSecurityConfig(AwsOpensearchserverlessDeleteSecurityConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSecurityPolicy(AwsOpensearchserverlessDeleteSecurityPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVpcEndpoint(AwsOpensearchserverlessDeleteVpcEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAccessPolicy(AwsOpensearchserverlessGetAccessPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAccountSettings(AwsOpensearchserverlessGetAccountSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpensearchserverlessGetAccountSettingsOptions(), token);
    }

    public async Task<CommandResult> GetPoliciesStats(AwsOpensearchserverlessGetPoliciesStatsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpensearchserverlessGetPoliciesStatsOptions(), token);
    }

    public async Task<CommandResult> GetSecurityConfig(AwsOpensearchserverlessGetSecurityConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSecurityPolicy(AwsOpensearchserverlessGetSecurityPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAccessPolicies(AwsOpensearchserverlessListAccessPoliciesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCollections(AwsOpensearchserverlessListCollectionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpensearchserverlessListCollectionsOptions(), token);
    }

    public async Task<CommandResult> ListLifecyclePolicies(AwsOpensearchserverlessListLifecyclePoliciesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSecurityConfigs(AwsOpensearchserverlessListSecurityConfigsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSecurityPolicies(AwsOpensearchserverlessListSecurityPoliciesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsOpensearchserverlessListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListVpcEndpoints(AwsOpensearchserverlessListVpcEndpointsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpensearchserverlessListVpcEndpointsOptions(), token);
    }

    public async Task<CommandResult> TagResource(AwsOpensearchserverlessTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsOpensearchserverlessUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAccessPolicy(AwsOpensearchserverlessUpdateAccessPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAccountSettings(AwsOpensearchserverlessUpdateAccountSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOpensearchserverlessUpdateAccountSettingsOptions(), token);
    }

    public async Task<CommandResult> UpdateCollection(AwsOpensearchserverlessUpdateCollectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateLifecyclePolicy(AwsOpensearchserverlessUpdateLifecyclePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSecurityConfig(AwsOpensearchserverlessUpdateSecurityConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSecurityPolicy(AwsOpensearchserverlessUpdateSecurityPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateVpcEndpoint(AwsOpensearchserverlessUpdateVpcEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
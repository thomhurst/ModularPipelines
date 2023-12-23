using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsCloudfront
{
    public AwsCloudfront(
        AwsCloudfrontWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsCloudfrontWait Wait { get; }

    public async Task<CommandResult> AssociateAlias(AwsCloudfrontAssociateAliasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopyDistribution(AwsCloudfrontCopyDistributionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCachePolicy(AwsCloudfrontCreateCachePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCloudFrontOriginAccessIdentity(AwsCloudfrontCreateCloudFrontOriginAccessIdentityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateContinuousDeploymentPolicy(AwsCloudfrontCreateContinuousDeploymentPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDistribution(AwsCloudfrontCreateDistributionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudfrontCreateDistributionOptions(), token);
    }

    public async Task<CommandResult> CreateDistributionWithTags(AwsCloudfrontCreateDistributionWithTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFieldLevelEncryptionConfig(AwsCloudfrontCreateFieldLevelEncryptionConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFieldLevelEncryptionProfile(AwsCloudfrontCreateFieldLevelEncryptionProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFunction(AwsCloudfrontCreateFunctionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateInvalidation(AwsCloudfrontCreateInvalidationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateKeyGroup(AwsCloudfrontCreateKeyGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateKeyValueStore(AwsCloudfrontCreateKeyValueStoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMonitoringSubscription(AwsCloudfrontCreateMonitoringSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateOriginAccessControl(AwsCloudfrontCreateOriginAccessControlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateOriginRequestPolicy(AwsCloudfrontCreateOriginRequestPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePublicKey(AwsCloudfrontCreatePublicKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRealtimeLogConfig(AwsCloudfrontCreateRealtimeLogConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateResponseHeadersPolicy(AwsCloudfrontCreateResponseHeadersPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateStreamingDistribution(AwsCloudfrontCreateStreamingDistributionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateStreamingDistributionWithTags(AwsCloudfrontCreateStreamingDistributionWithTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCachePolicy(AwsCloudfrontDeleteCachePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCloudFrontOriginAccessIdentity(AwsCloudfrontDeleteCloudFrontOriginAccessIdentityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteContinuousDeploymentPolicy(AwsCloudfrontDeleteContinuousDeploymentPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDistribution(AwsCloudfrontDeleteDistributionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFieldLevelEncryptionConfig(AwsCloudfrontDeleteFieldLevelEncryptionConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFieldLevelEncryptionProfile(AwsCloudfrontDeleteFieldLevelEncryptionProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFunction(AwsCloudfrontDeleteFunctionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteKeyGroup(AwsCloudfrontDeleteKeyGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteKeyValueStore(AwsCloudfrontDeleteKeyValueStoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMonitoringSubscription(AwsCloudfrontDeleteMonitoringSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteOriginAccessControl(AwsCloudfrontDeleteOriginAccessControlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteOriginRequestPolicy(AwsCloudfrontDeleteOriginRequestPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePublicKey(AwsCloudfrontDeletePublicKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRealtimeLogConfig(AwsCloudfrontDeleteRealtimeLogConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudfrontDeleteRealtimeLogConfigOptions(), token);
    }

    public async Task<CommandResult> DeleteResponseHeadersPolicy(AwsCloudfrontDeleteResponseHeadersPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteStreamingDistribution(AwsCloudfrontDeleteStreamingDistributionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFunction(AwsCloudfrontDescribeFunctionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeKeyValueStore(AwsCloudfrontDescribeKeyValueStoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCachePolicy(AwsCloudfrontGetCachePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCachePolicyConfig(AwsCloudfrontGetCachePolicyConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCloudFrontOriginAccessIdentity(AwsCloudfrontGetCloudFrontOriginAccessIdentityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCloudFrontOriginAccessIdentityConfig(AwsCloudfrontGetCloudFrontOriginAccessIdentityConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetContinuousDeploymentPolicy(AwsCloudfrontGetContinuousDeploymentPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetContinuousDeploymentPolicyConfig(AwsCloudfrontGetContinuousDeploymentPolicyConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDistribution(AwsCloudfrontGetDistributionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDistributionConfig(AwsCloudfrontGetDistributionConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFieldLevelEncryption(AwsCloudfrontGetFieldLevelEncryptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFieldLevelEncryptionConfig(AwsCloudfrontGetFieldLevelEncryptionConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFieldLevelEncryptionProfile(AwsCloudfrontGetFieldLevelEncryptionProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFieldLevelEncryptionProfileConfig(AwsCloudfrontGetFieldLevelEncryptionProfileConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFunction(AwsCloudfrontGetFunctionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInvalidation(AwsCloudfrontGetInvalidationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetKeyGroup(AwsCloudfrontGetKeyGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetKeyGroupConfig(AwsCloudfrontGetKeyGroupConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMonitoringSubscription(AwsCloudfrontGetMonitoringSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetOriginAccessControl(AwsCloudfrontGetOriginAccessControlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetOriginAccessControlConfig(AwsCloudfrontGetOriginAccessControlConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetOriginRequestPolicy(AwsCloudfrontGetOriginRequestPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetOriginRequestPolicyConfig(AwsCloudfrontGetOriginRequestPolicyConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPublicKey(AwsCloudfrontGetPublicKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPublicKeyConfig(AwsCloudfrontGetPublicKeyConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRealtimeLogConfig(AwsCloudfrontGetRealtimeLogConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudfrontGetRealtimeLogConfigOptions(), token);
    }

    public async Task<CommandResult> GetResponseHeadersPolicy(AwsCloudfrontGetResponseHeadersPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResponseHeadersPolicyConfig(AwsCloudfrontGetResponseHeadersPolicyConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetStreamingDistribution(AwsCloudfrontGetStreamingDistributionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetStreamingDistributionConfig(AwsCloudfrontGetStreamingDistributionConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCachePolicies(AwsCloudfrontListCachePoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudfrontListCachePoliciesOptions(), token);
    }

    public async Task<CommandResult> ListCloudFrontOriginAccessIdentities(AwsCloudfrontListCloudFrontOriginAccessIdentitiesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudfrontListCloudFrontOriginAccessIdentitiesOptions(), token);
    }

    public async Task<CommandResult> ListConflictingAliases(AwsCloudfrontListConflictingAliasesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListContinuousDeploymentPolicies(AwsCloudfrontListContinuousDeploymentPoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudfrontListContinuousDeploymentPoliciesOptions(), token);
    }

    public async Task<CommandResult> ListDistributions(AwsCloudfrontListDistributionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudfrontListDistributionsOptions(), token);
    }

    public async Task<CommandResult> ListDistributionsByCachePolicyId(AwsCloudfrontListDistributionsByCachePolicyIdOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDistributionsByKeyGroup(AwsCloudfrontListDistributionsByKeyGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDistributionsByOriginRequestPolicyId(AwsCloudfrontListDistributionsByOriginRequestPolicyIdOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDistributionsByRealtimeLogConfig(AwsCloudfrontListDistributionsByRealtimeLogConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudfrontListDistributionsByRealtimeLogConfigOptions(), token);
    }

    public async Task<CommandResult> ListDistributionsByResponseHeadersPolicyId(AwsCloudfrontListDistributionsByResponseHeadersPolicyIdOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDistributionsByWebAclId(AwsCloudfrontListDistributionsByWebAclIdOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListFieldLevelEncryptionConfigs(AwsCloudfrontListFieldLevelEncryptionConfigsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudfrontListFieldLevelEncryptionConfigsOptions(), token);
    }

    public async Task<CommandResult> ListFieldLevelEncryptionProfiles(AwsCloudfrontListFieldLevelEncryptionProfilesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudfrontListFieldLevelEncryptionProfilesOptions(), token);
    }

    public async Task<CommandResult> ListFunctions(AwsCloudfrontListFunctionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudfrontListFunctionsOptions(), token);
    }

    public async Task<CommandResult> ListInvalidations(AwsCloudfrontListInvalidationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListKeyGroups(AwsCloudfrontListKeyGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudfrontListKeyGroupsOptions(), token);
    }

    public async Task<CommandResult> ListKeyValueStores(AwsCloudfrontListKeyValueStoresOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudfrontListKeyValueStoresOptions(), token);
    }

    public async Task<CommandResult> ListOriginAccessControls(AwsCloudfrontListOriginAccessControlsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudfrontListOriginAccessControlsOptions(), token);
    }

    public async Task<CommandResult> ListOriginRequestPolicies(AwsCloudfrontListOriginRequestPoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudfrontListOriginRequestPoliciesOptions(), token);
    }

    public async Task<CommandResult> ListPublicKeys(AwsCloudfrontListPublicKeysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudfrontListPublicKeysOptions(), token);
    }

    public async Task<CommandResult> ListRealtimeLogConfigs(AwsCloudfrontListRealtimeLogConfigsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudfrontListRealtimeLogConfigsOptions(), token);
    }

    public async Task<CommandResult> ListResponseHeadersPolicies(AwsCloudfrontListResponseHeadersPoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudfrontListResponseHeadersPoliciesOptions(), token);
    }

    public async Task<CommandResult> ListStreamingDistributions(AwsCloudfrontListStreamingDistributionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudfrontListStreamingDistributionsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsCloudfrontListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PublishFunction(AwsCloudfrontPublishFunctionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Sign(AwsCloudfrontSignOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsCloudfrontTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TestFunction(AwsCloudfrontTestFunctionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsCloudfrontUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCachePolicy(AwsCloudfrontUpdateCachePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCloudFrontOriginAccessIdentity(AwsCloudfrontUpdateCloudFrontOriginAccessIdentityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateContinuousDeploymentPolicy(AwsCloudfrontUpdateContinuousDeploymentPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDistribution(AwsCloudfrontUpdateDistributionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDistributionWithStagingConfig(AwsCloudfrontUpdateDistributionWithStagingConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFieldLevelEncryptionConfig(AwsCloudfrontUpdateFieldLevelEncryptionConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFieldLevelEncryptionProfile(AwsCloudfrontUpdateFieldLevelEncryptionProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFunction(AwsCloudfrontUpdateFunctionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateKeyGroup(AwsCloudfrontUpdateKeyGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateKeyValueStore(AwsCloudfrontUpdateKeyValueStoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateOriginAccessControl(AwsCloudfrontUpdateOriginAccessControlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateOriginRequestPolicy(AwsCloudfrontUpdateOriginRequestPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePublicKey(AwsCloudfrontUpdatePublicKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRealtimeLogConfig(AwsCloudfrontUpdateRealtimeLogConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudfrontUpdateRealtimeLogConfigOptions(), token);
    }

    public async Task<CommandResult> UpdateResponseHeadersPolicy(AwsCloudfrontUpdateResponseHeadersPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateStreamingDistribution(AwsCloudfrontUpdateStreamingDistributionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
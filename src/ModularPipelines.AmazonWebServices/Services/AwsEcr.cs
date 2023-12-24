using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsEcr
{
    public AwsEcr(
        AwsEcrWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsEcrWait Wait { get; }

    public async Task<CommandResult> BatchCheckLayerAvailability(AwsEcrBatchCheckLayerAvailabilityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDeleteImage(AwsEcrBatchDeleteImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetImage(AwsEcrBatchGetImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetRepositoryScanningConfiguration(AwsEcrBatchGetRepositoryScanningConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CompleteLayerUpload(AwsEcrCompleteLayerUploadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePullThroughCacheRule(AwsEcrCreatePullThroughCacheRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRepository(AwsEcrCreateRepositoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLifecyclePolicy(AwsEcrDeleteLifecyclePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePullThroughCacheRule(AwsEcrDeletePullThroughCacheRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRegistryPolicy(AwsEcrDeleteRegistryPolicyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcrDeleteRegistryPolicyOptions(), token);
    }

    public async Task<CommandResult> DeleteRepository(AwsEcrDeleteRepositoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRepositoryPolicy(AwsEcrDeleteRepositoryPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeImageReplicationStatus(AwsEcrDescribeImageReplicationStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeImageScanFindings(AwsEcrDescribeImageScanFindingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeImages(AwsEcrDescribeImagesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribePullThroughCacheRules(AwsEcrDescribePullThroughCacheRulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcrDescribePullThroughCacheRulesOptions(), token);
    }

    public async Task<CommandResult> DescribeRegistry(AwsEcrDescribeRegistryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcrDescribeRegistryOptions(), token);
    }

    public async Task<CommandResult> DescribeRepositories(AwsEcrDescribeRepositoriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcrDescribeRepositoriesOptions(), token);
    }

    public async Task<CommandResult> GetAuthorizationToken(AwsEcrGetAuthorizationTokenOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcrGetAuthorizationTokenOptions(), token);
    }

    public async Task<CommandResult> GetDownloadUrlForLayer(AwsEcrGetDownloadUrlForLayerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLifecyclePolicy(AwsEcrGetLifecyclePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLifecyclePolicyPreview(AwsEcrGetLifecyclePolicyPreviewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLoginPassword(AwsEcrGetLoginPasswordOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcrGetLoginPasswordOptions(), token);
    }

    public async Task<CommandResult> GetRegistryPolicy(AwsEcrGetRegistryPolicyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcrGetRegistryPolicyOptions(), token);
    }

    public async Task<CommandResult> GetRegistryScanningConfiguration(AwsEcrGetRegistryScanningConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcrGetRegistryScanningConfigurationOptions(), token);
    }

    public async Task<CommandResult> GetRepositoryPolicy(AwsEcrGetRepositoryPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InitiateLayerUpload(AwsEcrInitiateLayerUploadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListImages(AwsEcrListImagesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsEcrListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutImage(AwsEcrPutImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutImageScanningConfiguration(AwsEcrPutImageScanningConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutImageTagMutability(AwsEcrPutImageTagMutabilityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutLifecyclePolicy(AwsEcrPutLifecyclePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutRegistryPolicy(AwsEcrPutRegistryPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutRegistryScanningConfiguration(AwsEcrPutRegistryScanningConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcrPutRegistryScanningConfigurationOptions(), token);
    }

    public async Task<CommandResult> PutReplicationConfiguration(AwsEcrPutReplicationConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetRepositoryPolicy(AwsEcrSetRepositoryPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartImageScan(AwsEcrStartImageScanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartLifecyclePolicyPreview(AwsEcrStartLifecyclePolicyPreviewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsEcrTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsEcrUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePullThroughCacheRule(AwsEcrUpdatePullThroughCacheRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UploadLayerPart(AwsEcrUploadLayerPartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ValidatePullThroughCacheRule(AwsEcrValidatePullThroughCacheRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
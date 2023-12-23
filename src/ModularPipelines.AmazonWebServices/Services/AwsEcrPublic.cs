using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsEcrPublic
{
    public AwsEcrPublic(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> BatchCheckLayerAvailability(AwsEcrPublicBatchCheckLayerAvailabilityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDeleteImage(AwsEcrPublicBatchDeleteImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CompleteLayerUpload(AwsEcrPublicCompleteLayerUploadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRepository(AwsEcrPublicCreateRepositoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRepository(AwsEcrPublicDeleteRepositoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRepositoryPolicy(AwsEcrPublicDeleteRepositoryPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeImageTags(AwsEcrPublicDescribeImageTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeImages(AwsEcrPublicDescribeImagesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRegistries(AwsEcrPublicDescribeRegistriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcrPublicDescribeRegistriesOptions(), token);
    }

    public async Task<CommandResult> DescribeRepositories(AwsEcrPublicDescribeRepositoriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcrPublicDescribeRepositoriesOptions(), token);
    }

    public async Task<CommandResult> GetAuthorizationToken(AwsEcrPublicGetAuthorizationTokenOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcrPublicGetAuthorizationTokenOptions(), token);
    }

    public async Task<CommandResult> GetLoginPassword(AwsEcrPublicGetLoginPasswordOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcrPublicGetLoginPasswordOptions(), token);
    }

    public async Task<CommandResult> GetRegistryCatalogData(AwsEcrPublicGetRegistryCatalogDataOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcrPublicGetRegistryCatalogDataOptions(), token);
    }

    public async Task<CommandResult> GetRepositoryCatalogData(AwsEcrPublicGetRepositoryCatalogDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRepositoryPolicy(AwsEcrPublicGetRepositoryPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InitiateLayerUpload(AwsEcrPublicInitiateLayerUploadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsEcrPublicListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutImage(AwsEcrPublicPutImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutRegistryCatalogData(AwsEcrPublicPutRegistryCatalogDataOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcrPublicPutRegistryCatalogDataOptions(), token);
    }

    public async Task<CommandResult> PutRepositoryCatalogData(AwsEcrPublicPutRepositoryCatalogDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetRepositoryPolicy(AwsEcrPublicSetRepositoryPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsEcrPublicTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsEcrPublicUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UploadLayerPart(AwsEcrPublicUploadLayerPartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
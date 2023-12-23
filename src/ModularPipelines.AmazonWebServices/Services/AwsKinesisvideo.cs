using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsKinesisvideo
{
    public AwsKinesisvideo(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateSignalingChannel(AwsKinesisvideoCreateSignalingChannelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateStream(AwsKinesisvideoCreateStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEdgeConfiguration(AwsKinesisvideoDeleteEdgeConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoDeleteEdgeConfigurationOptions(), token);
    }

    public async Task<CommandResult> DeleteSignalingChannel(AwsKinesisvideoDeleteSignalingChannelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteStream(AwsKinesisvideoDeleteStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEdgeConfiguration(AwsKinesisvideoDescribeEdgeConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoDescribeEdgeConfigurationOptions(), token);
    }

    public async Task<CommandResult> DescribeImageGenerationConfiguration(AwsKinesisvideoDescribeImageGenerationConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoDescribeImageGenerationConfigurationOptions(), token);
    }

    public async Task<CommandResult> DescribeMappedResourceConfiguration(AwsKinesisvideoDescribeMappedResourceConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoDescribeMappedResourceConfigurationOptions(), token);
    }

    public async Task<CommandResult> DescribeMediaStorageConfiguration(AwsKinesisvideoDescribeMediaStorageConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoDescribeMediaStorageConfigurationOptions(), token);
    }

    public async Task<CommandResult> DescribeNotificationConfiguration(AwsKinesisvideoDescribeNotificationConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoDescribeNotificationConfigurationOptions(), token);
    }

    public async Task<CommandResult> DescribeSignalingChannel(AwsKinesisvideoDescribeSignalingChannelOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoDescribeSignalingChannelOptions(), token);
    }

    public async Task<CommandResult> DescribeStream(AwsKinesisvideoDescribeStreamOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoDescribeStreamOptions(), token);
    }

    public async Task<CommandResult> GetDataEndpoint(AwsKinesisvideoGetDataEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSignalingChannelEndpoint(AwsKinesisvideoGetSignalingChannelEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListEdgeAgentConfigurations(AwsKinesisvideoListEdgeAgentConfigurationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSignalingChannels(AwsKinesisvideoListSignalingChannelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoListSignalingChannelsOptions(), token);
    }

    public async Task<CommandResult> ListStreams(AwsKinesisvideoListStreamsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoListStreamsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsKinesisvideoListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForStream(AwsKinesisvideoListTagsForStreamOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoListTagsForStreamOptions(), token);
    }

    public async Task<CommandResult> StartEdgeConfigurationUpdate(AwsKinesisvideoStartEdgeConfigurationUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsKinesisvideoTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagStream(AwsKinesisvideoTagStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsKinesisvideoUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagStream(AwsKinesisvideoUntagStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDataRetention(AwsKinesisvideoUpdateDataRetentionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateImageGenerationConfiguration(AwsKinesisvideoUpdateImageGenerationConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoUpdateImageGenerationConfigurationOptions(), token);
    }

    public async Task<CommandResult> UpdateMediaStorageConfiguration(AwsKinesisvideoUpdateMediaStorageConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateNotificationConfiguration(AwsKinesisvideoUpdateNotificationConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoUpdateNotificationConfigurationOptions(), token);
    }

    public async Task<CommandResult> UpdateSignalingChannel(AwsKinesisvideoUpdateSignalingChannelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateStream(AwsKinesisvideoUpdateStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
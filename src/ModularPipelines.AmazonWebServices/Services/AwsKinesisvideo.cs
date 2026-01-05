using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> CreateSignalingChannel(AwsKinesisvideoCreateSignalingChannelOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateStream(AwsKinesisvideoCreateStreamOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteEdgeConfiguration(AwsKinesisvideoDeleteEdgeConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoDeleteEdgeConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteSignalingChannel(AwsKinesisvideoDeleteSignalingChannelOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteStream(AwsKinesisvideoDeleteStreamOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEdgeConfiguration(AwsKinesisvideoDescribeEdgeConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoDescribeEdgeConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeImageGenerationConfiguration(AwsKinesisvideoDescribeImageGenerationConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoDescribeImageGenerationConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeMappedResourceConfiguration(AwsKinesisvideoDescribeMappedResourceConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoDescribeMappedResourceConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeMediaStorageConfiguration(AwsKinesisvideoDescribeMediaStorageConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoDescribeMediaStorageConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeNotificationConfiguration(AwsKinesisvideoDescribeNotificationConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoDescribeNotificationConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeSignalingChannel(AwsKinesisvideoDescribeSignalingChannelOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoDescribeSignalingChannelOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeStream(AwsKinesisvideoDescribeStreamOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoDescribeStreamOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetDataEndpoint(AwsKinesisvideoGetDataEndpointOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSignalingChannelEndpoint(AwsKinesisvideoGetSignalingChannelEndpointOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListEdgeAgentConfigurations(AwsKinesisvideoListEdgeAgentConfigurationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListSignalingChannels(AwsKinesisvideoListSignalingChannelsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoListSignalingChannelsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListStreams(AwsKinesisvideoListStreamsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoListStreamsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTagsForResource(AwsKinesisvideoListTagsForResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTagsForStream(AwsKinesisvideoListTagsForStreamOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoListTagsForStreamOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartEdgeConfigurationUpdate(AwsKinesisvideoStartEdgeConfigurationUpdateOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TagResource(AwsKinesisvideoTagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TagStream(AwsKinesisvideoTagStreamOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UntagResource(AwsKinesisvideoUntagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UntagStream(AwsKinesisvideoUntagStreamOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateDataRetention(AwsKinesisvideoUpdateDataRetentionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateImageGenerationConfiguration(AwsKinesisvideoUpdateImageGenerationConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoUpdateImageGenerationConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateMediaStorageConfiguration(AwsKinesisvideoUpdateMediaStorageConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateNotificationConfiguration(AwsKinesisvideoUpdateNotificationConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisvideoUpdateNotificationConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateSignalingChannel(AwsKinesisvideoUpdateSignalingChannelOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateStream(AwsKinesisvideoUpdateStreamOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}
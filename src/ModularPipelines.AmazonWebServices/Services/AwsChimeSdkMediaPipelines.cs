using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsChimeSdkMediaPipelines
{
    public AwsChimeSdkMediaPipelines(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateMediaCapturePipeline(AwsChimeSdkMediaPipelinesCreateMediaCapturePipelineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMediaConcatenationPipeline(AwsChimeSdkMediaPipelinesCreateMediaConcatenationPipelineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMediaInsightsPipeline(AwsChimeSdkMediaPipelinesCreateMediaInsightsPipelineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMediaInsightsPipelineConfiguration(AwsChimeSdkMediaPipelinesCreateMediaInsightsPipelineConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMediaLiveConnectorPipeline(AwsChimeSdkMediaPipelinesCreateMediaLiveConnectorPipelineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMediaPipelineKinesisVideoStreamPool(AwsChimeSdkMediaPipelinesCreateMediaPipelineKinesisVideoStreamPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMediaStreamPipeline(AwsChimeSdkMediaPipelinesCreateMediaStreamPipelineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMediaCapturePipeline(AwsChimeSdkMediaPipelinesDeleteMediaCapturePipelineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMediaInsightsPipelineConfiguration(AwsChimeSdkMediaPipelinesDeleteMediaInsightsPipelineConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMediaPipeline(AwsChimeSdkMediaPipelinesDeleteMediaPipelineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMediaPipelineKinesisVideoStreamPool(AwsChimeSdkMediaPipelinesDeleteMediaPipelineKinesisVideoStreamPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMediaCapturePipeline(AwsChimeSdkMediaPipelinesGetMediaCapturePipelineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMediaInsightsPipelineConfiguration(AwsChimeSdkMediaPipelinesGetMediaInsightsPipelineConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMediaPipeline(AwsChimeSdkMediaPipelinesGetMediaPipelineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMediaPipelineKinesisVideoStreamPool(AwsChimeSdkMediaPipelinesGetMediaPipelineKinesisVideoStreamPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSpeakerSearchTask(AwsChimeSdkMediaPipelinesGetSpeakerSearchTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVoiceToneAnalysisTask(AwsChimeSdkMediaPipelinesGetVoiceToneAnalysisTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListMediaCapturePipelines(AwsChimeSdkMediaPipelinesListMediaCapturePipelinesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkMediaPipelinesListMediaCapturePipelinesOptions(), token);
    }

    public async Task<CommandResult> ListMediaInsightsPipelineConfigurations(AwsChimeSdkMediaPipelinesListMediaInsightsPipelineConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkMediaPipelinesListMediaInsightsPipelineConfigurationsOptions(), token);
    }

    public async Task<CommandResult> ListMediaPipelineKinesisVideoStreamPools(AwsChimeSdkMediaPipelinesListMediaPipelineKinesisVideoStreamPoolsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkMediaPipelinesListMediaPipelineKinesisVideoStreamPoolsOptions(), token);
    }

    public async Task<CommandResult> ListMediaPipelines(AwsChimeSdkMediaPipelinesListMediaPipelinesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkMediaPipelinesListMediaPipelinesOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsChimeSdkMediaPipelinesListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartSpeakerSearchTask(AwsChimeSdkMediaPipelinesStartSpeakerSearchTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartVoiceToneAnalysisTask(AwsChimeSdkMediaPipelinesStartVoiceToneAnalysisTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopSpeakerSearchTask(AwsChimeSdkMediaPipelinesStopSpeakerSearchTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopVoiceToneAnalysisTask(AwsChimeSdkMediaPipelinesStopVoiceToneAnalysisTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsChimeSdkMediaPipelinesTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsChimeSdkMediaPipelinesUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateMediaInsightsPipelineConfiguration(AwsChimeSdkMediaPipelinesUpdateMediaInsightsPipelineConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateMediaInsightsPipelineStatus(AwsChimeSdkMediaPipelinesUpdateMediaInsightsPipelineStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateMediaPipelineKinesisVideoStreamPool(AwsChimeSdkMediaPipelinesUpdateMediaPipelineKinesisVideoStreamPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
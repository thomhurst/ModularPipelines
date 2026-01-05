using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> CreateMediaCapturePipeline(AwsChimeSdkMediaPipelinesCreateMediaCapturePipelineOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateMediaConcatenationPipeline(AwsChimeSdkMediaPipelinesCreateMediaConcatenationPipelineOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateMediaInsightsPipeline(AwsChimeSdkMediaPipelinesCreateMediaInsightsPipelineOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateMediaInsightsPipelineConfiguration(AwsChimeSdkMediaPipelinesCreateMediaInsightsPipelineConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateMediaLiveConnectorPipeline(AwsChimeSdkMediaPipelinesCreateMediaLiveConnectorPipelineOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateMediaPipelineKinesisVideoStreamPool(AwsChimeSdkMediaPipelinesCreateMediaPipelineKinesisVideoStreamPoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateMediaStreamPipeline(AwsChimeSdkMediaPipelinesCreateMediaStreamPipelineOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteMediaCapturePipeline(AwsChimeSdkMediaPipelinesDeleteMediaCapturePipelineOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteMediaInsightsPipelineConfiguration(AwsChimeSdkMediaPipelinesDeleteMediaInsightsPipelineConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteMediaPipeline(AwsChimeSdkMediaPipelinesDeleteMediaPipelineOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteMediaPipelineKinesisVideoStreamPool(AwsChimeSdkMediaPipelinesDeleteMediaPipelineKinesisVideoStreamPoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetMediaCapturePipeline(AwsChimeSdkMediaPipelinesGetMediaCapturePipelineOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetMediaInsightsPipelineConfiguration(AwsChimeSdkMediaPipelinesGetMediaInsightsPipelineConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetMediaPipeline(AwsChimeSdkMediaPipelinesGetMediaPipelineOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetMediaPipelineKinesisVideoStreamPool(AwsChimeSdkMediaPipelinesGetMediaPipelineKinesisVideoStreamPoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSpeakerSearchTask(AwsChimeSdkMediaPipelinesGetSpeakerSearchTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetVoiceToneAnalysisTask(AwsChimeSdkMediaPipelinesGetVoiceToneAnalysisTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListMediaCapturePipelines(AwsChimeSdkMediaPipelinesListMediaCapturePipelinesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkMediaPipelinesListMediaCapturePipelinesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListMediaInsightsPipelineConfigurations(AwsChimeSdkMediaPipelinesListMediaInsightsPipelineConfigurationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkMediaPipelinesListMediaInsightsPipelineConfigurationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListMediaPipelineKinesisVideoStreamPools(AwsChimeSdkMediaPipelinesListMediaPipelineKinesisVideoStreamPoolsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkMediaPipelinesListMediaPipelineKinesisVideoStreamPoolsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListMediaPipelines(AwsChimeSdkMediaPipelinesListMediaPipelinesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkMediaPipelinesListMediaPipelinesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTagsForResource(AwsChimeSdkMediaPipelinesListTagsForResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartSpeakerSearchTask(AwsChimeSdkMediaPipelinesStartSpeakerSearchTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartVoiceToneAnalysisTask(AwsChimeSdkMediaPipelinesStartVoiceToneAnalysisTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StopSpeakerSearchTask(AwsChimeSdkMediaPipelinesStopSpeakerSearchTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StopVoiceToneAnalysisTask(AwsChimeSdkMediaPipelinesStopVoiceToneAnalysisTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TagResource(AwsChimeSdkMediaPipelinesTagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UntagResource(AwsChimeSdkMediaPipelinesUntagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateMediaInsightsPipelineConfiguration(AwsChimeSdkMediaPipelinesUpdateMediaInsightsPipelineConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateMediaInsightsPipelineStatus(AwsChimeSdkMediaPipelinesUpdateMediaInsightsPipelineStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateMediaPipelineKinesisVideoStreamPool(AwsChimeSdkMediaPipelinesUpdateMediaPipelineKinesisVideoStreamPoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}
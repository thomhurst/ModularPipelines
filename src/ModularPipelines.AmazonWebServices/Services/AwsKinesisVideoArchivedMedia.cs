using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsKinesisVideoArchivedMedia
{
    public AwsKinesisVideoArchivedMedia(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> GetClip(AwsKinesisVideoArchivedMediaGetClipOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetDashStreamingSessionUrl(AwsKinesisVideoArchivedMediaGetDashStreamingSessionUrlOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisVideoArchivedMediaGetDashStreamingSessionUrlOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetHlsStreamingSessionUrl(AwsKinesisVideoArchivedMediaGetHlsStreamingSessionUrlOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisVideoArchivedMediaGetHlsStreamingSessionUrlOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetImages(AwsKinesisVideoArchivedMediaGetImagesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetMediaForFragmentList(AwsKinesisVideoArchivedMediaGetMediaForFragmentListOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListFragments(AwsKinesisVideoArchivedMediaListFragmentsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisVideoArchivedMediaListFragmentsOptions(), executionOptions, cancellationToken);
    }
}
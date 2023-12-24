using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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

    public async Task<CommandResult> GetClip(AwsKinesisVideoArchivedMediaGetClipOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDashStreamingSessionUrl(AwsKinesisVideoArchivedMediaGetDashStreamingSessionUrlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisVideoArchivedMediaGetDashStreamingSessionUrlOptions(), token);
    }

    public async Task<CommandResult> GetHlsStreamingSessionUrl(AwsKinesisVideoArchivedMediaGetHlsStreamingSessionUrlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisVideoArchivedMediaGetHlsStreamingSessionUrlOptions(), token);
    }

    public async Task<CommandResult> GetImages(AwsKinesisVideoArchivedMediaGetImagesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMediaForFragmentList(AwsKinesisVideoArchivedMediaGetMediaForFragmentListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListFragments(AwsKinesisVideoArchivedMediaListFragmentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisVideoArchivedMediaListFragmentsOptions(), token);
    }
}
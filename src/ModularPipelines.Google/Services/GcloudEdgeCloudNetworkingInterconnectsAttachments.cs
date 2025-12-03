using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "interconnects")]
public class GcloudEdgeCloudNetworkingInterconnectsAttachments
{
    public GcloudEdgeCloudNetworkingInterconnectsAttachments(
        GcloudEdgeCloudNetworkingInterconnectsAttachmentsDedicated dedicated,
        ICommand internalCommand
    )
    {
        Dedicated = dedicated;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudEdgeCloudNetworkingInterconnectsAttachmentsDedicated Dedicated { get; }

    public async Task<CommandResult> Delete(GcloudEdgeCloudNetworkingInterconnectsAttachmentsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudEdgeCloudNetworkingInterconnectsAttachmentsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudEdgeCloudNetworkingInterconnectsAttachmentsListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
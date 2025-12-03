using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "interconnects", "attachments")]
public class GcloudEdgeCloudNetworkingInterconnectsAttachmentsDedicated
{
    public GcloudEdgeCloudNetworkingInterconnectsAttachmentsDedicated(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(GcloudEdgeCloudNetworkingInterconnectsAttachmentsDedicatedCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking")]
public class GcloudEdgeCloudNetworkingInterconnects
{
    public GcloudEdgeCloudNetworkingInterconnects(
        GcloudEdgeCloudNetworkingInterconnectsAttachments attachments,
        ICommand internalCommand
    )
    {
        Attachments = attachments;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudEdgeCloudNetworkingInterconnectsAttachments Attachments { get; }

    public async Task<CommandResult> Describe(GcloudEdgeCloudNetworkingInterconnectsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDiagnostics(GcloudEdgeCloudNetworkingInterconnectsGetDiagnosticsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudEdgeCloudNetworkingInterconnectsListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
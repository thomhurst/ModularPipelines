using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "container")]
public class GcloudEdgeCloudContainerZones
{
    public GcloudEdgeCloudContainerZones(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Describe(GcloudEdgeCloudContainerZonesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudEdgeCloudContainerZonesListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudEdgeCloudContainerZonesListOptions(), token);
    }
}
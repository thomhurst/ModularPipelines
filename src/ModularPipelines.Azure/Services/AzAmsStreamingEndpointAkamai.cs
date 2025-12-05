using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("ams", "streaming-endpoint")]
public class AzAmsStreamingEndpointAkamai
{
    public AzAmsStreamingEndpointAkamai(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Add(AzAmsStreamingEndpointAkamaiAddOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsStreamingEndpointAkamaiAddOptions(), token);
    }

    public async Task<CommandResult> Remove(AzAmsStreamingEndpointAkamaiRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
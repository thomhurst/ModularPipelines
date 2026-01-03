using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("ams")]
public class AzAmsStreamingEndpoint
{
    public AzAmsStreamingEndpoint(
        AzAmsStreamingEndpointAkamai akamai,
        ICommand internalCommand
    )
    {
        Akamai = akamai;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAmsStreamingEndpointAkamai Akamai { get; }

    public async Task<CommandResult> Create(AzAmsStreamingEndpointCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzAmsStreamingEndpointDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsStreamingEndpointDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> GetSkus(AzAmsStreamingEndpointGetSkusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsStreamingEndpointGetSkusOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzAmsStreamingEndpointListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Scale(AzAmsStreamingEndpointScaleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzAmsStreamingEndpointShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsStreamingEndpointShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Start(AzAmsStreamingEndpointStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsStreamingEndpointStartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Stop(AzAmsStreamingEndpointStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsStreamingEndpointStopOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzAmsStreamingEndpointUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsStreamingEndpointUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzAmsStreamingEndpointWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsStreamingEndpointWaitOptions(), cancellationToken: token);
    }
}
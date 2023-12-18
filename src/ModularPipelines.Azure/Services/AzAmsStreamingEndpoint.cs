using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams")]
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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAmsStreamingEndpointDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsStreamingEndpointDeleteOptions(), token);
    }

    public async Task<CommandResult> GetSkus(AzAmsStreamingEndpointGetSkusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsStreamingEndpointGetSkusOptions(), token);
    }

    public async Task<CommandResult> List(AzAmsStreamingEndpointListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Scale(AzAmsStreamingEndpointScaleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzAmsStreamingEndpointShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsStreamingEndpointShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzAmsStreamingEndpointStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsStreamingEndpointStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzAmsStreamingEndpointStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsStreamingEndpointStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzAmsStreamingEndpointUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsStreamingEndpointUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzAmsStreamingEndpointWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsStreamingEndpointWaitOptions(), token);
    }
}
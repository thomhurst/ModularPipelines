using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door")]
public class AzNetworkFrontDoorBackendPool
{
    public AzNetworkFrontDoorBackendPool(
        AzNetworkFrontDoorBackendPoolBackend backend,
        ICommand internalCommand
    )
    {
        Backend = backend;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkFrontDoorBackendPoolBackend Backend { get; }

    public async Task<CommandResult> Create(AzNetworkFrontDoorBackendPoolCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkFrontDoorBackendPoolDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkFrontDoorBackendPoolListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkFrontDoorBackendPoolShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
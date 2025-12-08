using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("edgeorder")]
public class AzEdgeorderAddress
{
    public AzEdgeorderAddress(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzEdgeorderAddressCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEdgeorderAddressDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEdgeorderAddressDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzEdgeorderAddressListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEdgeorderAddressListOptions(), token);
    }

    public async Task<CommandResult> Show(AzEdgeorderAddressShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEdgeorderAddressShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzEdgeorderAddressUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEdgeorderAddressUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzEdgeorderAddressWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
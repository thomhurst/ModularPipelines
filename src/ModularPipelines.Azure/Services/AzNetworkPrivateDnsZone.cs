using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "private-dns")]
public class AzNetworkPrivateDnsZone
{
    public AzNetworkPrivateDnsZone(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkPrivateDnsZoneCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkPrivateDnsZoneDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Export(AzNetworkPrivateDnsZoneExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Import(AzNetworkPrivateDnsZoneImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkPrivateDnsZoneListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateDnsZoneListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkPrivateDnsZoneShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateDnsZoneShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkPrivateDnsZoneUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateDnsZoneUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkPrivateDnsZoneWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateDnsZoneWaitOptions(), token);
    }
}
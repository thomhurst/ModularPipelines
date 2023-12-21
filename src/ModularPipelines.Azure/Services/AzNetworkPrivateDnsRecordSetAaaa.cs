using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "private-dns", "record-set")]
public class AzNetworkPrivateDnsRecordSetAaaa
{
    public AzNetworkPrivateDnsRecordSetAaaa(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AddRecord(AzNetworkPrivateDnsRecordSetAaaaAddRecordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzNetworkPrivateDnsRecordSetAaaaCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkPrivateDnsRecordSetAaaaDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateDnsRecordSetAaaaDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkPrivateDnsRecordSetAaaaListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveRecord(AzNetworkPrivateDnsRecordSetAaaaRemoveRecordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkPrivateDnsRecordSetAaaaShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateDnsRecordSetAaaaShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkPrivateDnsRecordSetAaaaUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateDnsRecordSetAaaaUpdateOptions(), token);
    }
}
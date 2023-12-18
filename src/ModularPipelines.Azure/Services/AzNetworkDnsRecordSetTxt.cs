using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "dns", "record-set")]
public class AzNetworkDnsRecordSetTxt
{
    public AzNetworkDnsRecordSetTxt(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AddRecord(AzNetworkDnsRecordSetTxtAddRecordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzNetworkDnsRecordSetTxtCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkDnsRecordSetTxtDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkDnsRecordSetTxtDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkDnsRecordSetTxtListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveRecord(AzNetworkDnsRecordSetTxtRemoveRecordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkDnsRecordSetTxtShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkDnsRecordSetTxtShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkDnsRecordSetTxtUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkDnsRecordSetTxtUpdateOptions(), token);
    }
}
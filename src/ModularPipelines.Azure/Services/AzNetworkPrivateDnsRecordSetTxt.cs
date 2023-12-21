using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "private-dns", "record-set")]
public class AzNetworkPrivateDnsRecordSetTxt
{
    public AzNetworkPrivateDnsRecordSetTxt(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AddRecord(AzNetworkPrivateDnsRecordSetTxtAddRecordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzNetworkPrivateDnsRecordSetTxtCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkPrivateDnsRecordSetTxtDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateDnsRecordSetTxtDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkPrivateDnsRecordSetTxtListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveRecord(AzNetworkPrivateDnsRecordSetTxtRemoveRecordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkPrivateDnsRecordSetTxtShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateDnsRecordSetTxtShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkPrivateDnsRecordSetTxtUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateDnsRecordSetTxtUpdateOptions(), token);
    }
}
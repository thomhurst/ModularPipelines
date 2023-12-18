using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "dns", "record-set")]
public class AzNetworkDnsRecordSetDs
{
    public AzNetworkDnsRecordSetDs(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AddRecord(AzNetworkDnsRecordSetDsAddRecordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzNetworkDnsRecordSetDsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkDnsRecordSetDsDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkDnsRecordSetDsDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkDnsRecordSetDsListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveRecord(AzNetworkDnsRecordSetDsRemoveRecordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkDnsRecordSetDsShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkDnsRecordSetDsShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkDnsRecordSetDsUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkDnsRecordSetDsUpdateOptions(), token);
    }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "dns", "record-set")]
public class AzNetworkDnsRecordSetTlsa
{
    public AzNetworkDnsRecordSetTlsa(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AddRecord(AzNetworkDnsRecordSetTlsaAddRecordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzNetworkDnsRecordSetTlsaCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkDnsRecordSetTlsaDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkDnsRecordSetTlsaDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkDnsRecordSetTlsaListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveRecord(AzNetworkDnsRecordSetTlsaRemoveRecordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkDnsRecordSetTlsaShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkDnsRecordSetTlsaShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkDnsRecordSetTlsaUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkDnsRecordSetTlsaUpdateOptions(), token);
    }
}
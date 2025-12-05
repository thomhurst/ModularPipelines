using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "private-dns", "record-set")]
public class AzNetworkPrivateDnsRecordSetSoa
{
    public AzNetworkPrivateDnsRecordSetSoa(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Show(AzNetworkPrivateDnsRecordSetSoaShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateDnsRecordSetSoaShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkPrivateDnsRecordSetSoaUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
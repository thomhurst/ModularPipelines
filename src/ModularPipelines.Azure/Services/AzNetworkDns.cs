using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkDns
{
    public AzNetworkDns(
        AzNetworkDnsDnssecConfig dnssecConfig,
        AzNetworkDnsRecordSet recordSet,
        AzNetworkDnsZone zone,
        ICommand internalCommand
    )
    {
        DnssecConfig = dnssecConfig;
        RecordSet = recordSet;
        Zone = zone;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkDnsDnssecConfig DnssecConfig { get; }

    public AzNetworkDnsRecordSet RecordSet { get; }

    public AzNetworkDnsZone Zone { get; }

    public async Task<CommandResult> ListReferences(AzNetworkDnsListReferencesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkDnsListReferencesOptions(), token);
    }
}
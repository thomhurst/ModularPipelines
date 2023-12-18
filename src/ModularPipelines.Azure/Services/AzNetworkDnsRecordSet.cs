using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "dns")]
public class AzNetworkDnsRecordSet
{
    public AzNetworkDnsRecordSet(
        AzNetworkDnsRecordSetA a,
        AzNetworkDnsRecordSetAaaa aaaa,
        AzNetworkDnsRecordSetCaa caa,
        AzNetworkDnsRecordSetCname cname,
        AzNetworkDnsRecordSetDs ds,
        AzNetworkDnsRecordSetMx mx,
        AzNetworkDnsRecordSetNs ns,
        AzNetworkDnsRecordSetPtr ptr,
        AzNetworkDnsRecordSetSoa soa,
        AzNetworkDnsRecordSetSrv srv,
        AzNetworkDnsRecordSetTlsa tlsa,
        AzNetworkDnsRecordSetTxt txt,
        ICommand internalCommand
    )
    {
        A = a;
        Aaaa = aaaa;
        Caa = caa;
        Cname = cname;
        Ds = ds;
        Mx = mx;
        Ns = ns;
        Ptr = ptr;
        Soa = soa;
        Srv = srv;
        Tlsa = tlsa;
        Txt = txt;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkDnsRecordSetA A { get; }

    public AzNetworkDnsRecordSetAaaa Aaaa { get; }

    public AzNetworkDnsRecordSetCaa Caa { get; }

    public AzNetworkDnsRecordSetCname Cname { get; }

    public AzNetworkDnsRecordSetDs Ds { get; }

    public AzNetworkDnsRecordSetMx Mx { get; }

    public AzNetworkDnsRecordSetNs Ns { get; }

    public AzNetworkDnsRecordSetPtr Ptr { get; }

    public AzNetworkDnsRecordSetSoa Soa { get; }

    public AzNetworkDnsRecordSetSrv Srv { get; }

    public AzNetworkDnsRecordSetTlsa Tlsa { get; }

    public AzNetworkDnsRecordSetTxt Txt { get; }

    public async Task<CommandResult> List(AzNetworkDnsRecordSetListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}


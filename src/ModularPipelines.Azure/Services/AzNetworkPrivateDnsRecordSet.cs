using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "private-dns")]
public class AzNetworkPrivateDnsRecordSet
{
    public AzNetworkPrivateDnsRecordSet(
        AzNetworkPrivateDnsRecordSetA a,
        AzNetworkPrivateDnsRecordSetAaaa aaaa,
        AzNetworkPrivateDnsRecordSetCname cname,
        AzNetworkPrivateDnsRecordSetMx mx,
        AzNetworkPrivateDnsRecordSetPtr ptr,
        AzNetworkPrivateDnsRecordSetSoa soa,
        AzNetworkPrivateDnsRecordSetSrv srv,
        AzNetworkPrivateDnsRecordSetTxt txt,
        ICommand internalCommand
    )
    {
        A = a;
        Aaaa = aaaa;
        Cname = cname;
        Mx = mx;
        Ptr = ptr;
        Soa = soa;
        Srv = srv;
        Txt = txt;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkPrivateDnsRecordSetA A { get; }

    public AzNetworkPrivateDnsRecordSetAaaa Aaaa { get; }

    public AzNetworkPrivateDnsRecordSetCname Cname { get; }

    public AzNetworkPrivateDnsRecordSetMx Mx { get; }

    public AzNetworkPrivateDnsRecordSetPtr Ptr { get; }

    public AzNetworkPrivateDnsRecordSetSoa Soa { get; }

    public AzNetworkPrivateDnsRecordSetSrv Srv { get; }

    public AzNetworkPrivateDnsRecordSetTxt Txt { get; }

    public async Task<CommandResult> List(AzNetworkPrivateDnsRecordSetListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
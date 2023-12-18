using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "dns", "record-set", "cname", "set-record")]
public record AzNetworkDnsRecordSetCnameSetRecordOptions(
[property: CommandSwitch("--cname")] string Cname,
[property: CommandSwitch("--record-set-name")] string RecordSetName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--zone-name")] string ZoneName
) : AzOptions
{
    [CommandSwitch("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CommandSwitch("--ttl")]
    public string? Ttl { get; set; }
}
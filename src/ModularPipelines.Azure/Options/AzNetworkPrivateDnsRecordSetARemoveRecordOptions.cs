using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "private-dns", "record-set", "a", "remove-record")]
public record AzNetworkPrivateDnsRecordSetARemoveRecordOptions(
[property: CommandSwitch("--ipv4-address")] string Ipv4Address,
[property: CommandSwitch("--record-set-name")] string RecordSetName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--zone-name")] string ZoneName
) : AzOptions
{
    [BooleanCommandSwitch("--keep-empty-record-set")]
    public bool? KeepEmptyRecordSet { get; set; }
}
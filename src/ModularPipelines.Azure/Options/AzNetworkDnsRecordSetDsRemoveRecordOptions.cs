using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "dns", "record-set", "ds", "remove-record")]
public record AzNetworkDnsRecordSetDsRemoveRecordOptions(
[property: CommandSwitch("--algorithm")] string Algorithm,
[property: CommandSwitch("--digest")] string Digest,
[property: CommandSwitch("--digest-type")] string DigestType,
[property: CommandSwitch("--key-tag")] string KeyTag,
[property: CommandSwitch("--record-set-name")] string RecordSetName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--zone-name")] string ZoneName
) : AzOptions
{
    [BooleanCommandSwitch("--keep-empty-record-set")]
    public bool? KeepEmptyRecordSet { get; set; }
}
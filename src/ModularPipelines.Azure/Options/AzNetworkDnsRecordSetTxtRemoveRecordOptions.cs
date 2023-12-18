using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "dns", "record-set", "txt", "remove-record")]
public record AzNetworkDnsRecordSetTxtRemoveRecordOptions(
[property: CommandSwitch("--record-set-name")] string RecordSetName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--value")] string Value,
[property: CommandSwitch("--zone-name")] string ZoneName
) : AzOptions
{
    [BooleanCommandSwitch("--keep-empty-record-set")]
    public bool? KeepEmptyRecordSet { get; set; }
}
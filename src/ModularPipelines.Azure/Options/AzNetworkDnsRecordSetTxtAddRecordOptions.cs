using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "dns", "record-set", "txt", "add-record")]
public record AzNetworkDnsRecordSetTxtAddRecordOptions(
[property: CommandSwitch("--record-set-name")] string RecordSetName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--value")] string Value,
[property: CommandSwitch("--zone-name")] string ZoneName
) : AzOptions
{
    [CommandSwitch("--if-none-match")]
    public string? IfNoneMatch { get; set; }
}
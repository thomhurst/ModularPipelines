using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "dns", "record-set", "mx", "add-record")]
public record AzNetworkDnsRecordSetMxAddRecordOptions(
[property: CommandSwitch("--exchange")] string Exchange,
[property: CommandSwitch("--preference")] string Preference,
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
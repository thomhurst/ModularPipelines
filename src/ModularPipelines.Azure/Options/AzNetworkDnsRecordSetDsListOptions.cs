using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "dns", "record-set", "ds", "list")]
public record AzNetworkDnsRecordSetDsListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--zone-name")] string ZoneName
) : AzOptions
{
    [CommandSwitch("--recordsetnamesuffix")]
    public string? Recordsetnamesuffix { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}
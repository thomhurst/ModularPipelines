using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "dns", "record-set", "tlsa", "list")]
public record AzNetworkDnsRecordSetTlsaListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--zone-name")] string ZoneName
) : AzOptions
{
    [CommandSwitch("--recordsetnamesuffix")]
    public string? Recordsetnamesuffix { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}
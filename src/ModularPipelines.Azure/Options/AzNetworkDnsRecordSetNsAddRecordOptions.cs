using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "dns", "record-set", "ns", "add-record")]
public record AzNetworkDnsRecordSetNsAddRecordOptions(
[property: CommandSwitch("--nsdname")] string Nsdname,
[property: CommandSwitch("--record-set-name")] string RecordSetName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--zone-name")] string ZoneName
) : AzOptions
{
    [CommandSwitch("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CommandSwitch("--subscriptionid")]
    public string? Subscriptionid { get; set; }

    [CommandSwitch("--ttl")]
    public string? Ttl { get; set; }
}


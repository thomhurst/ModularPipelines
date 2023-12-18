using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "dns", "record-set", "ds", "add-record")]
public record AzNetworkDnsRecordSetDsAddRecordOptions(
[property: CommandSwitch("--algorithm")] string Algorithm,
[property: CommandSwitch("--digest")] string Digest,
[property: CommandSwitch("--digest-type")] string DigestType,
[property: CommandSwitch("--key-tag")] string KeyTag,
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
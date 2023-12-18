using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "dns", "record-set", "tlsa", "add-record")]
public record AzNetworkDnsRecordSetTlsaAddRecordOptions(
[property: CommandSwitch("--certificate-data")] string CertificateData,
[property: CommandSwitch("--certificate-usage")] string CertificateUsage,
[property: CommandSwitch("--matching-type")] string MatchingType,
[property: CommandSwitch("--record-set-name")] string RecordSetName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--selector")] string Selector,
[property: CommandSwitch("--zone-name")] string ZoneName
) : AzOptions
{
    [CommandSwitch("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CommandSwitch("--ttl")]
    public string? Ttl { get; set; }
}
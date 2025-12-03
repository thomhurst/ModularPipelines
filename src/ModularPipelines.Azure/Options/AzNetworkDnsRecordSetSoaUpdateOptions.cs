using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "dns", "record-set", "soa", "update")]
public record AzNetworkDnsRecordSetSoaUpdateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions
{
    [CliOption("--email")]
    public string? Email { get; set; }

    [CliOption("--expire-time")]
    public string? ExpireTime { get; set; }

    [CliOption("--host")]
    public string? Host { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--minimum-ttl")]
    public string? MinimumTtl { get; set; }

    [CliOption("--refresh-time")]
    public string? RefreshTime { get; set; }

    [CliOption("--retry-time")]
    public string? RetryTime { get; set; }

    [CliOption("--serial-number")]
    public string? SerialNumber { get; set; }
}
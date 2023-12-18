using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "dns", "record-set", "soa", "update")]
public record AzNetworkDnsRecordSetSoaUpdateOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--zone-name")] string ZoneName
) : AzOptions
{
    [CommandSwitch("--email")]
    public string? Email { get; set; }

    [CommandSwitch("--expire-time")]
    public string? ExpireTime { get; set; }

    [CommandSwitch("--host")]
    public string? Host { get; set; }

    [CommandSwitch("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CommandSwitch("--minimum-ttl")]
    public string? MinimumTtl { get; set; }

    [CommandSwitch("--refresh-time")]
    public string? RefreshTime { get; set; }

    [CommandSwitch("--retry-time")]
    public string? RetryTime { get; set; }

    [CommandSwitch("--serial-number")]
    public string? SerialNumber { get; set; }
}
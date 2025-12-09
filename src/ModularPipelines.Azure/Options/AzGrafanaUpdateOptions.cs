using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("grafana", "update")]
public record AzGrafanaUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--api-key")]
    public string? ApiKey { get; set; }

    [CliOption("--deterministic-outbound-ip")]
    public string? DeterministicOutboundIp { get; set; }

    [CliOption("--from-address")]
    public string? FromAddress { get; set; }

    [CliOption("--from-name")]
    public string? FromName { get; set; }

    [CliOption("--host")]
    public string? Host { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--skip-verify")]
    public bool? SkipVerify { get; set; }

    [CliOption("--smtp")]
    public string? Smtp { get; set; }

    [CliOption("--start-tls-policy")]
    public string? StartTlsPolicy { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--user")]
    public string? User { get; set; }
}
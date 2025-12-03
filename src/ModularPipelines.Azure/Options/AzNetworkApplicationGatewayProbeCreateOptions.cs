using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "probe", "create")]
public record AzNetworkApplicationGatewayProbeCreateOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--from-http-settings")]
    public bool? FromHttpSettings { get; set; }

    [CliFlag("--from-settings")]
    public bool? FromSettings { get; set; }

    [CliOption("--host")]
    public string? Host { get; set; }

    [CliOption("--interval")]
    public int? Interval { get; set; }

    [CliOption("--match-body")]
    public string? MatchBody { get; set; }

    [CliOption("--match-status-codes")]
    public string? MatchStatusCodes { get; set; }

    [CliOption("--min-servers")]
    public string? MinServers { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--threshold")]
    public string? Threshold { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}
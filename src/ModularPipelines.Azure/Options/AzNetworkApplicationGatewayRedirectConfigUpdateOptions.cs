using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "redirect-config", "update")]
public record AzNetworkApplicationGatewayRedirectConfigUpdateOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliFlag("--include-path")]
    public bool? IncludePath { get; set; }

    [CliFlag("--include-query-string")]
    public bool? IncludeQueryString { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--target-listener")]
    public string? TargetListener { get; set; }

    [CliOption("--target-url")]
    public string? TargetUrl { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}
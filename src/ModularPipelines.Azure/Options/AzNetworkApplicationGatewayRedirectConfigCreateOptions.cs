using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "redirect-config", "create")]
public record AzNetworkApplicationGatewayRedirectConfigCreateOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliFlag("--include-path")]
    public bool? IncludePath { get; set; }

    [CliFlag("--include-query-string")]
    public bool? IncludeQueryString { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--target-listener")]
    public string? TargetListener { get; set; }

    [CliOption("--target-url")]
    public string? TargetUrl { get; set; }
}
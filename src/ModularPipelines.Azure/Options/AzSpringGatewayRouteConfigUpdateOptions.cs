using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "gateway", "route-config", "update")]
public record AzSpringGatewayRouteConfigUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--app-name")]
    public string? AppName { get; set; }

    [CliOption("--routes-file")]
    public string? RoutesFile { get; set; }

    [CliOption("--routes-json")]
    public string? RoutesJson { get; set; }
}
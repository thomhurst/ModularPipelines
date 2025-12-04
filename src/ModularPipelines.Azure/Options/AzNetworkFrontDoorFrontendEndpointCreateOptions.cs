using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "frontend-endpoint", "create")]
public record AzNetworkFrontDoorFrontendEndpointCreateOptions(
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--host-name")] string HostName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--session-affinity-enabled")]
    public bool? SessionAffinityEnabled { get; set; }

    [CliOption("--session-affinity-ttl")]
    public string? SessionAffinityTtl { get; set; }

    [CliOption("--waf-policy")]
    public string? WafPolicy { get; set; }
}
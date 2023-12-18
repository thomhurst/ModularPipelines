using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "frontend-endpoint", "create")]
public record AzNetworkFrontDoorFrontendEndpointCreateOptions(
[property: CommandSwitch("--front-door-name")] string FrontDoorName,
[property: CommandSwitch("--host-name")] string HostName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--session-affinity-enabled")]
    public bool? SessionAffinityEnabled { get; set; }

    [CommandSwitch("--session-affinity-ttl")]
    public string? SessionAffinityTtl { get; set; }

    [CommandSwitch("--waf-policy")]
    public string? WafPolicy { get; set; }
}
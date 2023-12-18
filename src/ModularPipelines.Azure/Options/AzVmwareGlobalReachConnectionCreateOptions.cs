using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "global-reach-connection", "create")]
public record AzVmwareGlobalReachConnectionCreateOptions(
[property: CommandSwitch("--global-reach-connection-name")] string GlobalReachConnectionName,
[property: CommandSwitch("--private-cloud")] string PrivateCloud,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--authorization-key")]
    public string? AuthorizationKey { get; set; }

    [CommandSwitch("--express-route-id")]
    public string? ExpressRouteId { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--peer-express-route-circuit")]
    public string? PeerExpressRouteCircuit { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmware", "global-reach-connection", "create")]
public record AzVmwareGlobalReachConnectionCreateOptions(
[property: CliOption("--global-reach-connection-name")] string GlobalReachConnectionName,
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--authorization-key")]
    public string? AuthorizationKey { get; set; }

    [CliOption("--express-route-id")]
    public string? ExpressRouteId { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--peer-express-route-circuit")]
    public string? PeerExpressRouteCircuit { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "authorization", "create")]
public record AzVmwareAuthorizationCreateOptions(
[property: CliOption("--authorization-name")] string AuthorizationName,
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--express-route-id")]
    public string? ExpressRouteId { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}
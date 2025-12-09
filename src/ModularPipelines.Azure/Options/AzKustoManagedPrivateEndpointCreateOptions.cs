using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("kusto", "managed-private-endpoint", "create")]
public record AzKustoManagedPrivateEndpointCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--managed-private-endpoint-name")] string ManagedPrivateEndpointName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--group-id")]
    public string? GroupId { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--private-link")]
    public string? PrivateLink { get; set; }

    [CliOption("--private-link-region")]
    public string? PrivateLinkRegion { get; set; }

    [CliOption("--request-message")]
    public string? RequestMessage { get; set; }
}
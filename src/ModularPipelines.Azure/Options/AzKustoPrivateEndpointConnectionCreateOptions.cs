using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("kusto", "private-endpoint-connection", "create")]
public record AzKustoPrivateEndpointConnectionCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--connection-state")]
    public string? ConnectionState { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}
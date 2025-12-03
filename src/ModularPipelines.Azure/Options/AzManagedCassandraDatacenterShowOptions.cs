using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managed-cassandra", "datacenter", "show")]
public record AzManagedCassandraDatacenterShowOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--data-center-name")] string DataCenterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;
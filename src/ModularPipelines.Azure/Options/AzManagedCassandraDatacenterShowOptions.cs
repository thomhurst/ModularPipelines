using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managed-cassandra", "datacenter", "show")]
public record AzManagedCassandraDatacenterShowOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--data-center-name")] string DataCenterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;
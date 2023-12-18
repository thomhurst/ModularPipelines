using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "cluster-principal-assignment", "list")]
public record AzKustoClusterPrincipalAssignmentListOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;
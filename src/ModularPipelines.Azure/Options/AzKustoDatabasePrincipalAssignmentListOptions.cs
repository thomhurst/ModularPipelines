using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("kusto", "database-principal-assignment", "list")]
public record AzKustoDatabasePrincipalAssignmentListOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;
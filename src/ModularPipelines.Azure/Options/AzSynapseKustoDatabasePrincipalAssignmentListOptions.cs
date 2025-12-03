using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "kusto", "database-principal-assignment", "list")]
public record AzSynapseKustoDatabasePrincipalAssignmentListOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--kusto-pool-name")] string KustoPoolName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;
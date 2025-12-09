using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "kusto", "attached-database-configuration", "list")]
public record AzSynapseKustoAttachedDatabaseConfigurationListOptions(
[property: CliOption("--kusto-pool-name")] string KustoPoolName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("managed-cassandra", "cluster", "backup", "show")]
public record AzManagedCassandraClusterBackupShowOptions(
[property: CliOption("--backup-id")] string BackupId,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;
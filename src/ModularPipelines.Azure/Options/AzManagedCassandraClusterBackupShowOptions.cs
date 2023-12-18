using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managed-cassandra", "cluster", "backup", "show")]
public record AzManagedCassandraClusterBackupShowOptions(
[property: CommandSwitch("--backup-id")] string BackupId,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;
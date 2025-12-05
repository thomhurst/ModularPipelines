using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataprotection", "backup-instance", "initialize-backupconfig")]
public record AzDataprotectionBackupInstanceInitializeBackupconfigOptions(
[property: CliOption("--datasource-type")] string DatasourceType
) : AzOptions
{
    [CliOption("--backup-hook-references")]
    public string? BackupHookReferences { get; set; }

    [CliOption("--excluded-namespaces")]
    public string? ExcludedNamespaces { get; set; }

    [CliOption("--excluded-resource-type")]
    public string? ExcludedResourceType { get; set; }

    [CliFlag("--include-cluster-scope")]
    public bool? IncludeClusterScope { get; set; }

    [CliOption("--included-namespaces")]
    public string? IncludedNamespaces { get; set; }

    [CliOption("--included-resource-type")]
    public string? IncludedResourceType { get; set; }

    [CliOption("--label-selectors")]
    public string? LabelSelectors { get; set; }

    [CliFlag("--snapshot-volumes")]
    public bool? SnapshotVolumes { get; set; }
}
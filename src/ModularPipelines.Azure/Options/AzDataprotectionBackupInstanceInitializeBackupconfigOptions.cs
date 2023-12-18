using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-instance", "initialize-backupconfig")]
public record AzDataprotectionBackupInstanceInitializeBackupconfigOptions(
[property: CommandSwitch("--datasource-type")] string DatasourceType
) : AzOptions
{
    [CommandSwitch("--backup-hook-references")]
    public string? BackupHookReferences { get; set; }

    [CommandSwitch("--excluded-namespaces")]
    public string? ExcludedNamespaces { get; set; }

    [CommandSwitch("--excluded-resource-type")]
    public string? ExcludedResourceType { get; set; }

    [BooleanCommandSwitch("--include-cluster-scope")]
    public bool? IncludeClusterScope { get; set; }

    [CommandSwitch("--included-namespaces")]
    public string? IncludedNamespaces { get; set; }

    [CommandSwitch("--included-resource-type")]
    public string? IncludedResourceType { get; set; }

    [CommandSwitch("--label-selectors")]
    public string? LabelSelectors { get; set; }

    [BooleanCommandSwitch("--snapshot-volumes")]
    public bool? SnapshotVolumes { get; set; }
}
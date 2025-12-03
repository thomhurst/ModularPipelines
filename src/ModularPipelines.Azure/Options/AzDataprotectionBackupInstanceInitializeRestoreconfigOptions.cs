using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataprotection", "backup-instance", "initialize-restoreconfig")]
public record AzDataprotectionBackupInstanceInitializeRestoreconfigOptions(
[property: CliOption("--datasource-type")] string DatasourceType
) : AzOptions
{
    [CliOption("--conflict-policy")]
    public string? ConflictPolicy { get; set; }

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

    [CliOption("--namespace-mappings")]
    public string? NamespaceMappings { get; set; }

    [CliOption("--persistent-restoremode")]
    public string? PersistentRestoremode { get; set; }

    [CliOption("--restore-hook-references")]
    public string? RestoreHookReferences { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-instance", "initialize-restoreconfig")]
public record AzDataprotectionBackupInstanceInitializeRestoreconfigOptions(
[property: CommandSwitch("--datasource-type")] string DatasourceType
) : AzOptions
{
    [CommandSwitch("--conflict-policy")]
    public string? ConflictPolicy { get; set; }

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

    [CommandSwitch("--namespace-mappings")]
    public string? NamespaceMappings { get; set; }

    [CommandSwitch("--persistent-restoremode")]
    public string? PersistentRestoremode { get; set; }

    [CommandSwitch("--restore-hook-references")]
    public string? RestoreHookReferences { get; set; }
}


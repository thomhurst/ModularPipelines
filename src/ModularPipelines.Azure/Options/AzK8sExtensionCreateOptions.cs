using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("k8s-extension", "create")]
public record AzK8sExtensionCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--cluster-type")] string ClusterType,
[property: CliOption("--extension-type")] string ExtensionType,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--auto-upgrade")]
    public bool? AutoUpgrade { get; set; }

    [CliOption("--cluster-resource-provider")]
    public string? ClusterResourceProvider { get; set; }

    [CliOption("--config")]
    public string? Config { get; set; }

    [CliOption("--config-file")]
    public string? ConfigFile { get; set; }

    [CliOption("--config-protected")]
    public string? ConfigProtected { get; set; }

    [CliOption("--config-protected-file")]
    public string? ConfigProtectedFile { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--plan-name")]
    public string? PlanName { get; set; }

    [CliOption("--plan-product")]
    public string? PlanProduct { get; set; }

    [CliOption("--plan-publisher")]
    public string? PlanPublisher { get; set; }

    [CliOption("--release-namespace")]
    public string? ReleaseNamespace { get; set; }

    [CliOption("--release-train")]
    public string? ReleaseTrain { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--target-namespace")]
    public string? TargetNamespace { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}
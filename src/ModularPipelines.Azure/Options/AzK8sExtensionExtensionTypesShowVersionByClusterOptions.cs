using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("k8s-extension", "extension-types", "show-version-by-cluster")]
public record AzK8sExtensionExtensionTypesShowVersionByClusterOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--cluster-type")] string ClusterType,
[property: CliOption("--extension-type")] string ExtensionType,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--version")] string Version
) : AzOptions;
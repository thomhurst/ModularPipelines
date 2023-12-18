using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("k8s-extension", "extension-types", "show-by-cluster")]
public record AzK8sExtensionExtensionTypesShowByClusterOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--cluster-type")] string ClusterType,
[property: CommandSwitch("--extension-type")] string ExtensionType,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;
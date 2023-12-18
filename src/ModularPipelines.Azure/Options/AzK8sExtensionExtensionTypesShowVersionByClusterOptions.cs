using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("k8s-extension", "extension-types", "show-version-by-cluster")]
public record AzK8sExtensionExtensionTypesShowVersionByClusterOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--cluster-type")] string ClusterType,
[property: CommandSwitch("--extension-type")] string ExtensionType,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--version")] string Version
) : AzOptions
{
}
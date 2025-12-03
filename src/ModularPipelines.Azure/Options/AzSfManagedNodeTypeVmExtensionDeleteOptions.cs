using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sf", "managed-node-type", "vm-extension", "delete")]
public record AzSfManagedNodeTypeVmExtensionDeleteOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--extension-name")] string ExtensionName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;
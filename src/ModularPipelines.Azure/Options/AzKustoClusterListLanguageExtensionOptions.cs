using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "cluster", "list-language-extension")]
public record AzKustoClusterListLanguageExtensionOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;
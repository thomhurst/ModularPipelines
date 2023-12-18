using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "cluster", "list")]
public record AzKustoClusterListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;
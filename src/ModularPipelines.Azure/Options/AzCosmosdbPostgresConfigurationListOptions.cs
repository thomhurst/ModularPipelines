using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "postgres", "configuration", "list")]
public record AzCosmosdbPostgresConfigurationListOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;
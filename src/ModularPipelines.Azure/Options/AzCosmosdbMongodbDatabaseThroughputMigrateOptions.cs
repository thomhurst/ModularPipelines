using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb", "database", "throughput", "migrate")]
public record AzCosmosdbMongodbDatabaseThroughputMigrateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--throughput-type")] string ThroughputType
) : AzOptions;
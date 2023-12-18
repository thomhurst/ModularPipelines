using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb", "restorable-database", "list")]
public record AzCosmosdbMongodbRestorableDatabaseListOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--location")] string Location
) : AzOptions;
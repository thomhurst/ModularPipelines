using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql", "restorable-database", "list")]
public record AzCosmosdbSqlRestorableDatabaseListOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--location")] string Location
) : AzOptions;
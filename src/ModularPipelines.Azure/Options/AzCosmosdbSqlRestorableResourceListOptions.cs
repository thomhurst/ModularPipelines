using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql", "restorable-resource", "list")]
public record AzCosmosdbSqlRestorableResourceListOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--restore-location")] string RestoreLocation,
[property: CommandSwitch("--restore-timestamp")] string RestoreTimestamp
) : AzOptions;
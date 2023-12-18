using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "postgres", "check-name-availability")]
public record AzCosmosdbPostgresCheckNameAvailabilityOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--type")] string Type
) : AzOptions;
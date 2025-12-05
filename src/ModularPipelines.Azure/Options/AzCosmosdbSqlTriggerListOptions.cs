using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "sql", "trigger", "list")]
public record AzCosmosdbSqlTriggerListOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--container-name")] string ContainerName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;
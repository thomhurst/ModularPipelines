using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "sql", "stored-procedure", "create")]
public record AzCosmosdbSqlStoredProcedureCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--body")] string Body,
[property: CliOption("--container-name")] string ContainerName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;
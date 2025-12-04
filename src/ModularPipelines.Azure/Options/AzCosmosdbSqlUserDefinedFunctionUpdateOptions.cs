using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "sql", "user-defined-function", "update")]
public record AzCosmosdbSqlUserDefinedFunctionUpdateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--body")] string Body,
[property: CliOption("--container-name")] string ContainerName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;
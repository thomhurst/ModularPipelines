using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql", "user-defined-function", "update")]
public record AzCosmosdbSqlUserDefinedFunctionUpdateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--body")] string Body,
[property: CommandSwitch("--container-name")] string ContainerName,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;
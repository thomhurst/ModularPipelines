using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb", "user", "definition", "create")]
public record AzCosmosdbMongodbUserDefinitionCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--body")] string Body,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;
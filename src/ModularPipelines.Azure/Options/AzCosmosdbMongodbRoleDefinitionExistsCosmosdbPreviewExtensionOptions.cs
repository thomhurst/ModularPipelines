using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb", "role", "definition", "exists", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbMongodbRoleDefinitionExistsCosmosdbPreviewExtensionOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;
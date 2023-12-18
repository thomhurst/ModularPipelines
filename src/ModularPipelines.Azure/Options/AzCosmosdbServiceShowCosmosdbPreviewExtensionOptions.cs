using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "service", "show", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbServiceShowCosmosdbPreviewExtensionOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group-name")] string ResourceGroupName
) : AzOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "service", "delete", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbServiceDeleteCosmosdbPreviewExtensionOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group-name")] string ResourceGroupName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}
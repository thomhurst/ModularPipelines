using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "service", "create", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbServiceCreateCosmosdbPreviewExtensionOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--kind")] string Kind,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group-name")] string ResourceGroupName
) : AzOptions
{
    [CliOption("--count")]
    public int? Count { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--size")]
    public string? Size { get; set; }
}
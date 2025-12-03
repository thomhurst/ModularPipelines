using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "service", "update", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbServiceUpdateCosmosdbPreviewExtensionOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--count")] int Count,
[property: CliOption("--kind")] string Kind,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group-name")] string ResourceGroupName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--size")]
    public string? Size { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "service", "update")]
public record AzCosmosdbServiceUpdateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--count")] int Count,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group-name")] string ResourceGroupName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--size")]
    public string? Size { get; set; }
}
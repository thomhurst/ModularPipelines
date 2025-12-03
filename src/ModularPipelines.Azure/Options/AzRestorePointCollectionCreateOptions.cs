using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("restore-point", "collection", "create")]
public record AzRestorePointCollectionCreateOptions(
[property: CliOption("--collection-name")] string CollectionName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--source-id")] string SourceId
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}
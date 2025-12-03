using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("restore-point", "collection", "show")]
public record AzRestorePointCollectionShowOptions(
[property: CliOption("--collection-name")] string CollectionName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--restore-points")]
    public string? RestorePoints { get; set; }
}
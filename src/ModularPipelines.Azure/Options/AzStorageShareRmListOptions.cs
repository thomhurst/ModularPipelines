using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "share-rm", "list")]
public record AzStorageShareRmListOptions(
[property: CliOption("--storage-account")] int StorageAccount
) : AzOptions
{
    [CliOption("--include-deleted")]
    public string? IncludeDeleted { get; set; }

    [CliOption("--include-snapshot")]
    public string? IncludeSnapshot { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}
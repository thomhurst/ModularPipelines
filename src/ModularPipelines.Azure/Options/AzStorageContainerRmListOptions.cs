using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "container-rm", "list")]
public record AzStorageContainerRmListOptions(
[property: CliOption("--storage-account")] int StorageAccount
) : AzOptions
{
    [CliOption("--include-deleted")]
    public string? IncludeDeleted { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}
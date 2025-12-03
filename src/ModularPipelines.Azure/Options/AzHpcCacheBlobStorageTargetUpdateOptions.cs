using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hpc-cache", "blob-storage-target", "update")]
public record AzHpcCacheBlobStorageTargetUpdateOptions(
[property: CliOption("--cache-name")] string CacheName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--virtual-namespace-path")]
    public string? VirtualNamespacePath { get; set; }
}
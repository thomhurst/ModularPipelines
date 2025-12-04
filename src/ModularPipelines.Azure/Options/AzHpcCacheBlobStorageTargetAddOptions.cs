using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("hpc-cache", "blob-storage-target", "add")]
public record AzHpcCacheBlobStorageTargetAddOptions(
[property: CliOption("--cache-name")] string CacheName,
[property: CliOption("--container-name")] string ContainerName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-account")] int StorageAccount,
[property: CliOption("--virtual-namespace-path")] string VirtualNamespacePath
) : AzOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hpc-cache", "blob-storage-target", "add")]
public record AzHpcCacheBlobStorageTargetAddOptions(
[property: CommandSwitch("--cache-name")] string CacheName,
[property: CommandSwitch("--container-name")] string ContainerName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--storage-account")] int StorageAccount,
[property: CommandSwitch("--virtual-namespace-path")] string VirtualNamespacePath
) : AzOptions
{
}
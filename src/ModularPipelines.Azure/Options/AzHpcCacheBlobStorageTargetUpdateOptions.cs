using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hpc-cache", "blob-storage-target", "update")]
public record AzHpcCacheBlobStorageTargetUpdateOptions(
[property: CommandSwitch("--cache-name")] string CacheName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--container-name")]
    public string? ContainerName { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }

    [CommandSwitch("--virtual-namespace-path")]
    public string? VirtualNamespacePath { get; set; }
}


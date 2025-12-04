using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("hpc-cache", "nfs-storage-target", "update")]
public record AzHpcCacheNfsStorageTargetUpdateOptions(
[property: CliOption("--cache-name")] string CacheName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--junction")]
    public string? Junction { get; set; }

    [CliOption("--nfs3-target")]
    public string? Nfs3Target { get; set; }

    [CliOption("--nfs3-usage-model")]
    public string? Nfs3UsageModel { get; set; }
}
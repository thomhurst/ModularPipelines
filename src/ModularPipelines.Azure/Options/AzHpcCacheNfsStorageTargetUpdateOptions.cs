using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hpc-cache", "nfs-storage-target", "update")]
public record AzHpcCacheNfsStorageTargetUpdateOptions(
[property: CommandSwitch("--cache-name")] string CacheName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--junction")]
    public string? Junction { get; set; }

    [CommandSwitch("--nfs3-target")]
    public string? Nfs3Target { get; set; }

    [CommandSwitch("--nfs3-usage-model")]
    public string? Nfs3UsageModel { get; set; }
}
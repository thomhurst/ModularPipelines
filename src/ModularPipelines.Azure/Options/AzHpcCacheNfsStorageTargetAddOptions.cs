using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hpc-cache", "nfs-storage-target", "add")]
public record AzHpcCacheNfsStorageTargetAddOptions(
[property: CommandSwitch("--cache-name")] string CacheName,
[property: CommandSwitch("--junction")] string Junction,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--nfs3-target")] string Nfs3Target,
[property: CommandSwitch("--nfs3-usage-model")] string Nfs3UsageModel,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}


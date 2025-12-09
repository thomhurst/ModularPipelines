using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("hpc-cache", "nfs-storage-target", "add")]
public record AzHpcCacheNfsStorageTargetAddOptions(
[property: CliOption("--cache-name")] string CacheName,
[property: CliOption("--junction")] string Junction,
[property: CliOption("--name")] string Name,
[property: CliOption("--nfs3-target")] string Nfs3Target,
[property: CliOption("--nfs3-usage-model")] string Nfs3UsageModel,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;
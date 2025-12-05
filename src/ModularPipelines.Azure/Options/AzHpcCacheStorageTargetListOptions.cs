using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("hpc-cache", "storage-target", "list")]
public record AzHpcCacheStorageTargetListOptions(
[property: CliOption("--cache-name")] string CacheName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;
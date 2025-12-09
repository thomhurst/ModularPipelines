using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("hpc-cache", "storage-target", "show")]
public record AzHpcCacheStorageTargetShowOptions(
[property: CliOption("--cache-name")] string CacheName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;
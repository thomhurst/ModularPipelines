using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hpc-cache", "storage-target", "remove")]
public record AzHpcCacheStorageTargetRemoveOptions(
[property: CliOption("--cache-name")] string CacheName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;
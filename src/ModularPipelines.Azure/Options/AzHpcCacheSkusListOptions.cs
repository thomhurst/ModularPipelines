using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hpc-cache", "skus", "list")]
public record AzHpcCacheSkusListOptions : AzOptions;
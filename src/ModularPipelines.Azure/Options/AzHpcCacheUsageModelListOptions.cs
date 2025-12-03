using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hpc-cache", "usage-model", "list")]
public record AzHpcCacheUsageModelListOptions : AzOptions;
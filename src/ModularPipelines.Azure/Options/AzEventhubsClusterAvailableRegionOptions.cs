using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventhubs", "cluster", "available-region")]
public record AzEventhubsClusterAvailableRegionOptions : AzOptions;
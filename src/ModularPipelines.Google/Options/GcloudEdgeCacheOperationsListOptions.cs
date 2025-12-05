using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cache", "operations", "list")]
public record GcloudEdgeCacheOperationsListOptions : GcloudOptions;
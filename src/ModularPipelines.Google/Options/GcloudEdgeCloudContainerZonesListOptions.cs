using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "container", "zones", "list")]
public record GcloudEdgeCloudContainerZonesListOptions : GcloudOptions;
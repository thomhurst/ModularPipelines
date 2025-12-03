using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "container", "regions", "list")]
public record GcloudEdgeCloudContainerRegionsListOptions : GcloudOptions;
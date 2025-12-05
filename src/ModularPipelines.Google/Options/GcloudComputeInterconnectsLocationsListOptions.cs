using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "interconnects", "locations", "list")]
public record GcloudComputeInterconnectsLocationsListOptions : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "interconnects", "remote-locations", "list")]
public record GcloudComputeInterconnectsRemoteLocationsListOptions : GcloudOptions;
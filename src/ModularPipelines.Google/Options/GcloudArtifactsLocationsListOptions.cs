using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "locations", "list")]
public record GcloudArtifactsLocationsListOptions : GcloudOptions;
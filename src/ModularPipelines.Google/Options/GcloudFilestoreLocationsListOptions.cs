using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("filestore", "locations", "list")]
public record GcloudFilestoreLocationsListOptions : GcloudOptions;
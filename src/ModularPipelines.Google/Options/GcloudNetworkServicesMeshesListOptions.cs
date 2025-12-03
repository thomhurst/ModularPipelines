using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "meshes", "list")]
public record GcloudNetworkServicesMeshesListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;
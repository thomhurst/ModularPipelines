using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "meshes", "describe")]
public record GcloudNetworkServicesMeshesDescribeOptions(
[property: CliArgument] string Mesh,
[property: CliArgument] string Location
) : GcloudOptions;
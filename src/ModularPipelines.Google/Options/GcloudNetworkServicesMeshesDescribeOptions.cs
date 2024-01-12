using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-services", "meshes", "describe")]
public record GcloudNetworkServicesMeshesDescribeOptions(
[property: PositionalArgument] string Mesh,
[property: PositionalArgument] string Location
) : GcloudOptions;
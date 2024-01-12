using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-services", "meshes", "list")]
public record GcloudNetworkServicesMeshesListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "networks", "describe")]
public record GcloudVmwareNetworksDescribeOptions(
[property: PositionalArgument] string VmwareEngineNetwork,
[property: PositionalArgument] string Location
) : GcloudOptions;
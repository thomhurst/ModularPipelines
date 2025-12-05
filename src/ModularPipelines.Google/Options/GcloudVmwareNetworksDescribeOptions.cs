using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "networks", "describe")]
public record GcloudVmwareNetworksDescribeOptions(
[property: CliArgument] string VmwareEngineNetwork,
[property: CliArgument] string Location
) : GcloudOptions;
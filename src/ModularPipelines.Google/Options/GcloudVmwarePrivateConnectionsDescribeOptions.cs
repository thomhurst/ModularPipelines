using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-connections", "describe")]
public record GcloudVmwarePrivateConnectionsDescribeOptions(
[property: PositionalArgument] string PrivateConnection,
[property: PositionalArgument] string Location
) : GcloudOptions;
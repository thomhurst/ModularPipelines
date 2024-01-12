using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-management", "connectivity-tests", "describe")]
public record GcloudNetworkManagementConnectivityTestsDescribeOptions(
[property: PositionalArgument] string ConnectivityTest
) : GcloudOptions;
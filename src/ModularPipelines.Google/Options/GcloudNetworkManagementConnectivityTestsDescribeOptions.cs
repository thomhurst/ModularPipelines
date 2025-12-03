using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-management", "connectivity-tests", "describe")]
public record GcloudNetworkManagementConnectivityTestsDescribeOptions(
[property: CliArgument] string ConnectivityTest
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "operations", "describe")]
public record GcloudNetworkConnectivityOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Region
) : GcloudOptions;
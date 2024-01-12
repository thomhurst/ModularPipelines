using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "operations", "describe")]
public record GcloudNetworkConnectivityOperationsDescribeOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Region
) : GcloudOptions;
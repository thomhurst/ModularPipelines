using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "internal-ranges", "describe")]
public record GcloudNetworkConnectivityInternalRangesDescribeOptions(
[property: PositionalArgument] string InternalRange,
[property: PositionalArgument] string Region
) : GcloudOptions;
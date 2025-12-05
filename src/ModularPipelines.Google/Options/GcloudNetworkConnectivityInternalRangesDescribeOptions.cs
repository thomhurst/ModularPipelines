using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "internal-ranges", "describe")]
public record GcloudNetworkConnectivityInternalRangesDescribeOptions(
[property: CliArgument] string InternalRange,
[property: CliArgument] string Region
) : GcloudOptions;
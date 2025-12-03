using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "networks", "describe")]
public record GcloudBmsNetworksDescribeOptions(
[property: CliArgument] string Network,
[property: CliArgument] string Region
) : GcloudOptions;
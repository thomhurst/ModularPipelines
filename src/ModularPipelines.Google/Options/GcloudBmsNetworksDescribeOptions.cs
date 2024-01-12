using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "networks", "describe")]
public record GcloudBmsNetworksDescribeOptions(
[property: PositionalArgument] string Network,
[property: PositionalArgument] string Region
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventarc", "channel-connections", "describe")]
public record GcloudEventarcChannelConnectionsDescribeOptions(
[property: PositionalArgument] string ChannelConnection,
[property: PositionalArgument] string Location
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventarc", "channel-connections", "describe")]
public record GcloudEventarcChannelConnectionsDescribeOptions(
[property: CliArgument] string ChannelConnection,
[property: CliArgument] string Location
) : GcloudOptions;
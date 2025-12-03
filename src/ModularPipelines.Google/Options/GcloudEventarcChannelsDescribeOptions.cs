using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventarc", "channels", "describe")]
public record GcloudEventarcChannelsDescribeOptions(
[property: CliArgument] string Channel,
[property: CliArgument] string Location
) : GcloudOptions;
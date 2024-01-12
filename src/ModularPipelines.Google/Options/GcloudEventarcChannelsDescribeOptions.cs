using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventarc", "channels", "describe")]
public record GcloudEventarcChannelsDescribeOptions(
[property: PositionalArgument] string Channel,
[property: PositionalArgument] string Location
) : GcloudOptions;
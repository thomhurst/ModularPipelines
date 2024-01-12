using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventarc", "channel-connections", "create")]
public record GcloudEventarcChannelConnectionsCreateOptions(
[property: PositionalArgument] string ChannelConnection,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--activation-token")] string ActivationToken,
[property: CommandSwitch("--channel")] string Channel
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}
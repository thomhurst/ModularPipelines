using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventarc", "channel-connections", "create")]
public record GcloudEventarcChannelConnectionsCreateOptions(
[property: CliArgument] string ChannelConnection,
[property: CliArgument] string Location,
[property: CliOption("--activation-token")] string ActivationToken,
[property: CliOption("--channel")] string Channel
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}
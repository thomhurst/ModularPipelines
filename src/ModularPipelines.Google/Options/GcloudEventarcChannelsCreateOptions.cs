using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventarc", "channels", "create")]
public record GcloudEventarcChannelsCreateOptions(
[property: PositionalArgument] string Channel,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--crypto-key")]
    public string? CryptoKey { get; set; }

    [CommandSwitch("--provider")]
    public string? Provider { get; set; }
}
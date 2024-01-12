using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventarc", "channels", "update")]
public record GcloudEventarcChannelsUpdateOptions(
[property: PositionalArgument] string Channel,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--clear-crypto-key")]
    public bool? ClearCryptoKey { get; set; }

    [CommandSwitch("--crypto-key")]
    public string? CryptoKey { get; set; }
}
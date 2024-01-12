using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventarc", "google-channels", "update")]
public record GcloudEventarcGoogleChannelsUpdateOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--clear-crypto-key")]
    public bool? ClearCryptoKey { get; set; }

    [CommandSwitch("--crypto-key")]
    public string? CryptoKey { get; set; }
}
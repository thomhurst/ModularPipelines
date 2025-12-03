using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventarc", "google-channels", "update")]
public record GcloudEventarcGoogleChannelsUpdateOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--clear-crypto-key")]
    public bool? ClearCryptoKey { get; set; }

    [CliOption("--crypto-key")]
    public string? CryptoKey { get; set; }
}
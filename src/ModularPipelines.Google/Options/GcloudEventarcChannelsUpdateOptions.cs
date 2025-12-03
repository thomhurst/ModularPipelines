using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventarc", "channels", "update")]
public record GcloudEventarcChannelsUpdateOptions(
[property: CliArgument] string Channel,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--clear-crypto-key")]
    public bool? ClearCryptoKey { get; set; }

    [CliOption("--crypto-key")]
    public string? CryptoKey { get; set; }
}
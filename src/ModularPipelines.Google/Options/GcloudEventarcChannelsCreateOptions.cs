using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventarc", "channels", "create")]
public record GcloudEventarcChannelsCreateOptions(
[property: CliArgument] string Channel,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--crypto-key")]
    public string? CryptoKey { get; set; }

    [CliOption("--provider")]
    public string? Provider { get; set; }
}
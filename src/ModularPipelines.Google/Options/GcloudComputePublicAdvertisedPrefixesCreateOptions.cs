using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "public-advertised-prefixes", "create")]
public record GcloudComputePublicAdvertisedPrefixesCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--dns-verification-ip")] string DnsVerificationIp,
[property: CliOption("--range")] string Range
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }
}
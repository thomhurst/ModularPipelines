using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "public-advertised-prefixes", "create")]
public record GcloudComputePublicAdvertisedPrefixesCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--dns-verification-ip")] string DnsVerificationIp,
[property: CommandSwitch("--range")] string Range
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "policies", "update")]
public record GcloudDnsPoliciesUpdateOptions(
[property: CliArgument] string Policy
) : GcloudOptions
{
    [CliOption("--alternative-name-servers")]
    public string[]? AlternativeNameServers { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable-inbound-forwarding")]
    public bool? EnableInboundForwarding { get; set; }

    [CliFlag("--enable-logging")]
    public bool? EnableLogging { get; set; }

    [CliOption("--networks")]
    public string[]? Networks { get; set; }

    [CliOption("--private-alternative-name-servers")]
    public string[]? PrivateAlternativeNameServers { get; set; }
}
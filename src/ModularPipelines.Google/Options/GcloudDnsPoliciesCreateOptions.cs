using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "policies", "create")]
public record GcloudDnsPoliciesCreateOptions(
[property: CliArgument] string Policy,
[property: CliOption("--description")] string Description,
[property: CliOption("--networks")] string[] Networks
) : GcloudOptions
{
    [CliOption("--alternative-name-servers")]
    public string[]? AlternativeNameServers { get; set; }

    [CliFlag("--enable-inbound-forwarding")]
    public bool? EnableInboundForwarding { get; set; }

    [CliFlag("--enable-logging")]
    public bool? EnableLogging { get; set; }

    [CliOption("--private-alternative-name-servers")]
    public string[]? PrivateAlternativeNameServers { get; set; }
}
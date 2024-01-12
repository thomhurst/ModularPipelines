using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "policies", "update")]
public record GcloudDnsPoliciesUpdateOptions(
[property: PositionalArgument] string Policy
) : GcloudOptions
{
    [CommandSwitch("--alternative-name-servers")]
    public string[]? AlternativeNameServers { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enable-inbound-forwarding")]
    public bool? EnableInboundForwarding { get; set; }

    [BooleanCommandSwitch("--enable-logging")]
    public bool? EnableLogging { get; set; }

    [CommandSwitch("--networks")]
    public string[]? Networks { get; set; }

    [CommandSwitch("--private-alternative-name-servers")]
    public string[]? PrivateAlternativeNameServers { get; set; }
}
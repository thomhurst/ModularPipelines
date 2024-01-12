using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "policies", "create")]
public record GcloudDnsPoliciesCreateOptions(
[property: PositionalArgument] string Policy,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--networks")] string[] Networks
) : GcloudOptions
{
    [CommandSwitch("--alternative-name-servers")]
    public string[]? AlternativeNameServers { get; set; }

    [BooleanCommandSwitch("--enable-inbound-forwarding")]
    public bool? EnableInboundForwarding { get; set; }

    [BooleanCommandSwitch("--enable-logging")]
    public bool? EnableLogging { get; set; }

    [CommandSwitch("--private-alternative-name-servers")]
    public string[]? PrivateAlternativeNameServers { get; set; }
}
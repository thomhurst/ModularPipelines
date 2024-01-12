using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "firewall-rules", "test-ip")]
public record GcloudAppFirewallRulesTestIpOptions(
[property: PositionalArgument] string Ip
) : GcloudOptions;
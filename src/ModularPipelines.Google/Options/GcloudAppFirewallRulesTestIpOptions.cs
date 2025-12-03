using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "firewall-rules", "test-ip")]
public record GcloudAppFirewallRulesTestIpOptions(
[property: CliArgument] string Ip
) : GcloudOptions;
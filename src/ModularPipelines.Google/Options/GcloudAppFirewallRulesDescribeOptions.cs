using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "firewall-rules", "describe")]
public record GcloudAppFirewallRulesDescribeOptions(
[property: CliArgument] string Priority
) : GcloudOptions;
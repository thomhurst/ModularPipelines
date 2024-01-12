using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "firewall-rules", "describe")]
public record GcloudAppFirewallRulesDescribeOptions(
[property: PositionalArgument] string Priority
) : GcloudOptions;
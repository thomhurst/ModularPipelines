using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "firewall-rules", "describe")]
public record GcloudComputeFirewallRulesDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;
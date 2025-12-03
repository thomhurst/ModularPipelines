using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "firewall-rules", "describe")]
public record GcloudComputeFirewallRulesDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions;
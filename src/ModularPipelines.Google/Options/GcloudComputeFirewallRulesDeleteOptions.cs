using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "firewall-rules", "delete")]
public record GcloudComputeFirewallRulesDeleteOptions(
[property: CliArgument] string Name
) : GcloudOptions;
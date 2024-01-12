using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "firewall-rules", "delete")]
public record GcloudComputeFirewallRulesDeleteOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;
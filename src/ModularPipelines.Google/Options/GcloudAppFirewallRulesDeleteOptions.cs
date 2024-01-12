using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "firewall-rules", "delete")]
public record GcloudAppFirewallRulesDeleteOptions(
[property: PositionalArgument] string Priority
) : GcloudOptions;
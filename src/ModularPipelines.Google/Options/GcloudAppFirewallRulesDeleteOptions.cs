using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "firewall-rules", "delete")]
public record GcloudAppFirewallRulesDeleteOptions(
[property: CliArgument] string Priority
) : GcloudOptions;
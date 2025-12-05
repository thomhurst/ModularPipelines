using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "firewall-rules", "list")]
public record GcloudAppFirewallRulesListOptions : GcloudOptions;
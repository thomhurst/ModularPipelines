using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "firewall-rules", "create")]
public record GcloudAppFirewallRulesCreateOptions(
[property: CliArgument] string Priority,
[property: CliOption("--action")] string Action,
[property: CliOption("--source-range")] string SourceRange
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }
}
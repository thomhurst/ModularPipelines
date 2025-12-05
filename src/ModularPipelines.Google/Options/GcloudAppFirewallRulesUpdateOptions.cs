using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "firewall-rules", "update")]
public record GcloudAppFirewallRulesUpdateOptions(
[property: CliArgument] string Priority
) : GcloudOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--source-range")]
    public string? SourceRange { get; set; }
}
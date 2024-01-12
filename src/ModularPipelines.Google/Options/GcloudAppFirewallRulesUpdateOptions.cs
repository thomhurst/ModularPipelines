using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "firewall-rules", "update")]
public record GcloudAppFirewallRulesUpdateOptions(
[property: PositionalArgument] string Priority
) : GcloudOptions
{
    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--source-range")]
    public string? SourceRange { get; set; }
}
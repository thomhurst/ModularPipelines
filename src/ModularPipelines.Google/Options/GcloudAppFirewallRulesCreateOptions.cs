using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "firewall-rules", "create")]
public record GcloudAppFirewallRulesCreateOptions(
[property: PositionalArgument] string Priority,
[property: CommandSwitch("--action")] string Action,
[property: CommandSwitch("--source-range")] string SourceRange
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "tags", "holds", "create")]
public record GcloudResourceManagerTagsHoldsCreateOptions(
[property: PositionalArgument] string Parent,
[property: CommandSwitch("--holder")] string Holder
) : GcloudOptions
{
    [CommandSwitch("--help-link")]
    public string? HelpLink { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--origin")]
    public string? Origin { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "tags", "holds", "list")]
public record GcloudResourceManagerTagsHoldsListOptions(
[property: PositionalArgument] string Parent
) : GcloudOptions
{
    [CommandSwitch("--holder")]
    public string? Holder { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--origin")]
    public string? Origin { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "levels", "update")]
public record GcloudAccessContextManagerLevelsUpdateOptions(
[property: PositionalArgument] string Level,
[property: PositionalArgument] string Policy
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--title")]
    public string? Title { get; set; }

    [CommandSwitch("--custom-level-spec")]
    public string? CustomLevelSpec { get; set; }

    [CommandSwitch("--basic-level-spec")]
    public string? BasicLevelSpec { get; set; }

    [CommandSwitch("--combine-function")]
    public string? CombineFunction { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "levels", "create")]
public record GcloudAccessContextManagerLevelsCreateOptions(
[property: PositionalArgument] string Level,
[property: PositionalArgument] string Policy,
[property: CommandSwitch("--title")] string Title,
[property: CommandSwitch("--custom-level-spec")] string CustomLevelSpec,
[property: CommandSwitch("--basic-level-spec")] string BasicLevelSpec,
[property: CommandSwitch("--combine-function")] string CombineFunction
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }
}
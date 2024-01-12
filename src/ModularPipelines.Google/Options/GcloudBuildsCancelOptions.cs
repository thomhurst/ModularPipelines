using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "cancel")]
public record GcloudBuildsCancelOptions(
[property: PositionalArgument] string Builds
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}
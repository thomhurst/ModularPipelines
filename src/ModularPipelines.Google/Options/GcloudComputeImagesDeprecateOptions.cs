using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "images", "deprecate")]
public record GcloudComputeImagesDeprecateOptions(
[property: PositionalArgument] string ImageName,
[property: CommandSwitch("--state")] string State
) : GcloudOptions
{
    [CommandSwitch("--replacement")]
    public string? Replacement { get; set; }

    [CommandSwitch("--delete-in")]
    public string? DeleteIn { get; set; }

    [CommandSwitch("--delete-on")]
    public string? DeleteOn { get; set; }

    [CommandSwitch("--deprecate-in")]
    public string? DeprecateIn { get; set; }

    [CommandSwitch("--deprecate-on")]
    public string? DeprecateOn { get; set; }

    [CommandSwitch("--obsolete-in")]
    public string? ObsoleteIn { get; set; }

    [CommandSwitch("--obsolete-on")]
    public string? ObsoleteOn { get; set; }
}
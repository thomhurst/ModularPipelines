using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "disks", "remove-labels")]
public record GcloudComputeDisksRemoveLabelsOptions(
[property: PositionalArgument] string DiskName,
[property: BooleanCommandSwitch("--all")] bool All,
[property: CommandSwitch("--labels")] string[] Labels
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}
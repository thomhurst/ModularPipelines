using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "remove-labels")]
public record GcloudComputeInstancesRemoveLabelsOptions(
[property: PositionalArgument] string InstanceName,
[property: BooleanCommandSwitch("--all")] bool All,
[property: CommandSwitch("--labels")] string[] Labels
) : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}
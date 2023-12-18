using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("footprint", "experiment", "create")]
public record AzFootprintExperimentCreateOptions(
[property: CommandSwitch("--experiment-name")] string ExperimentName,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }
}
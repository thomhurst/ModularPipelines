using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("evidently", "stop-experiment")]
public record AwsEvidentlyStopExperimentOptions(
[property: CommandSwitch("--experiment")] string Experiment,
[property: CommandSwitch("--project")] string Project
) : AwsOptions
{
    [CommandSwitch("--desired-state")]
    public string? DesiredState { get; set; }

    [CommandSwitch("--reason")]
    public string? Reason { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
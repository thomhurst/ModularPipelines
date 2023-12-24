using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("evidently", "get-experiment")]
public record AwsEvidentlyGetExperimentOptions(
[property: CommandSwitch("--experiment")] string Experiment,
[property: CommandSwitch("--project")] string Project
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
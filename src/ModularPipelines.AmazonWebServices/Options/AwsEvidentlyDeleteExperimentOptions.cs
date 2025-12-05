using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("evidently", "delete-experiment")]
public record AwsEvidentlyDeleteExperimentOptions(
[property: CliOption("--experiment")] string Experiment,
[property: CliOption("--project")] string Project
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
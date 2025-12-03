using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("application-insights", "update-problem")]
public record AwsApplicationInsightsUpdateProblemOptions(
[property: CliOption("--problem-id")] string ProblemId
) : AwsOptions
{
    [CliOption("--update-status")]
    public string? UpdateStatus { get; set; }

    [CliOption("--visibility")]
    public string? Visibility { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
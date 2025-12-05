using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "describe-solution-version")]
public record AwsPersonalizeDescribeSolutionVersionOptions(
[property: CliOption("--solution-version-arn")] string SolutionVersionArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
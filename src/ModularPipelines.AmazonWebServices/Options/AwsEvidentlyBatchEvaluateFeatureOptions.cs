using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("evidently", "batch-evaluate-feature")]
public record AwsEvidentlyBatchEvaluateFeatureOptions(
[property: CliOption("--project")] string Project,
[property: CliOption("--requests")] string[] Requests
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
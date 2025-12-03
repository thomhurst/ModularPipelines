using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "describe-step")]
public record AwsEmrDescribeStepOptions(
[property: CliOption("--cluster-id")] string ClusterId,
[property: CliOption("--step-id")] string StepId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
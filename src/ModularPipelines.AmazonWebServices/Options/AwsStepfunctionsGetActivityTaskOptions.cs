using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stepfunctions", "get-activity-task")]
public record AwsStepfunctionsGetActivityTaskOptions(
[property: CliOption("--activity-arn")] string ActivityArn
) : AwsOptions
{
    [CliOption("--worker-name")]
    public string? WorkerName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
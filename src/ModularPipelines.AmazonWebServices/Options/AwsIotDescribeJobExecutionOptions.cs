using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "describe-job-execution")]
public record AwsIotDescribeJobExecutionOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--thing-name")] string ThingName
) : AwsOptions
{
    [CliOption("--execution-number")]
    public long? ExecutionNumber { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
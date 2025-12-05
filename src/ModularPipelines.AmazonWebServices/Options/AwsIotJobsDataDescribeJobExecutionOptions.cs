using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot-jobs-data", "describe-job-execution")]
public record AwsIotJobsDataDescribeJobExecutionOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--thing-name")] string ThingName
) : AwsOptions
{
    [CliOption("--execution-number")]
    public long? ExecutionNumber { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
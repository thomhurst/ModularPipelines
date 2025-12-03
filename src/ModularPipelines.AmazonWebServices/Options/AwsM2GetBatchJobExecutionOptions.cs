using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("m2", "get-batch-job-execution")]
public record AwsM2GetBatchJobExecutionOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--execution-id")] string ExecutionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
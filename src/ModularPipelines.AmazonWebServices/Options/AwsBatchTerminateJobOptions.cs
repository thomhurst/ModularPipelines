using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "terminate-job")]
public record AwsBatchTerminateJobOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--reason")] string Reason
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
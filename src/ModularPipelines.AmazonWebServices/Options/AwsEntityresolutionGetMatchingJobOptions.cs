using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("entityresolution", "get-matching-job")]
public record AwsEntityresolutionGetMatchingJobOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--workflow-name")] string WorkflowName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
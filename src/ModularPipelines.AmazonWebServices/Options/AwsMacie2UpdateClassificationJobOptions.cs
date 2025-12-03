using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("macie2", "update-classification-job")]
public record AwsMacie2UpdateClassificationJobOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--job-status")] string JobStatus
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
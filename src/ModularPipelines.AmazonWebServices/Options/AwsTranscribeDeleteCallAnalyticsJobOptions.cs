using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "delete-call-analytics-job")]
public record AwsTranscribeDeleteCallAnalyticsJobOptions(
[property: CliOption("--call-analytics-job-name")] string CallAnalyticsJobName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "get-call-analytics-job")]
public record AwsTranscribeGetCallAnalyticsJobOptions(
[property: CliOption("--call-analytics-job-name")] string CallAnalyticsJobName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcribe", "get-call-analytics-job")]
public record AwsTranscribeGetCallAnalyticsJobOptions(
[property: CommandSwitch("--call-analytics-job-name")] string CallAnalyticsJobName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
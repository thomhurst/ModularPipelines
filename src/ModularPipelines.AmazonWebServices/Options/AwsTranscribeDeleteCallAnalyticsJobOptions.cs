using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcribe", "delete-call-analytics-job")]
public record AwsTranscribeDeleteCallAnalyticsJobOptions(
[property: CommandSwitch("--call-analytics-job-name")] string CallAnalyticsJobName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
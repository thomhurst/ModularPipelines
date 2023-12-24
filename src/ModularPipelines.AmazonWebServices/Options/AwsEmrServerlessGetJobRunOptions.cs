using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr-serverless", "get-job-run")]
public record AwsEmrServerlessGetJobRunOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--job-run-id")] string JobRunId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
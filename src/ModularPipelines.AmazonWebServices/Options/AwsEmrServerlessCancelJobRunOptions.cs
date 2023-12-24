using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr-serverless", "cancel-job-run")]
public record AwsEmrServerlessCancelJobRunOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--job-run-id")] string JobRunId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
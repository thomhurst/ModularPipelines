using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "update-job-priority")]
public record AwsS3controlUpdateJobPriorityOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--priority")] int Priority
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
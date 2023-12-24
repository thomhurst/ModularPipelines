using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "create-batch-segment-job")]
public record AwsPersonalizeCreateBatchSegmentJobOptions(
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--solution-version-arn")] string SolutionVersionArn,
[property: CommandSwitch("--job-input")] string JobInput,
[property: CommandSwitch("--job-output")] string JobOutput,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--filter-arn")]
    public string? FilterArn { get; set; }

    [CommandSwitch("--num-results")]
    public int? NumResults { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
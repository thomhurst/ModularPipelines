using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "describe-job-execution")]
public record AwsIotDescribeJobExecutionOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--thing-name")] string ThingName
) : AwsOptions
{
    [CommandSwitch("--execution-number")]
    public long? ExecutionNumber { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
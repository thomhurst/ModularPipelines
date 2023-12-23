using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "terminate-job")]
public record AwsBatchTerminateJobOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--reason")] string Reason
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
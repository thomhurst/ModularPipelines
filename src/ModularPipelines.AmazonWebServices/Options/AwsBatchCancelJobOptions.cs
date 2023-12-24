using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "cancel-job")]
public record AwsBatchCancelJobOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--reason")] string Reason
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
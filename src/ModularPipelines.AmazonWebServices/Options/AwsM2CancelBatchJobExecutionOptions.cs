using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("m2", "cancel-batch-job-execution")]
public record AwsM2CancelBatchJobExecutionOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--execution-id")] string ExecutionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
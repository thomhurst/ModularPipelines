using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "cancel-task-execution")]
public record AwsDatasyncCancelTaskExecutionOptions(
[property: CommandSwitch("--task-execution-arn")] string TaskExecutionArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
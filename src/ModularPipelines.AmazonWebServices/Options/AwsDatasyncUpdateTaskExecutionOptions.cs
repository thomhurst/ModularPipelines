using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "update-task-execution")]
public record AwsDatasyncUpdateTaskExecutionOptions(
[property: CommandSwitch("--task-execution-arn")] string TaskExecutionArn,
[property: CommandSwitch("--options")] string Options
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
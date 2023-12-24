using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("swf", "describe-workflow-execution")]
public record AwsSwfDescribeWorkflowExecutionOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--execution")] string Execution
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
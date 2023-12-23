using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker-a2i-runtime", "describe-human-loop")]
public record AwsSagemakerA2iRuntimeDescribeHumanLoopOptions(
[property: CommandSwitch("--human-loop-name")] string HumanLoopName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
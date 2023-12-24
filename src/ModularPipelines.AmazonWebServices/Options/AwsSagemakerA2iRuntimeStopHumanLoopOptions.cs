using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker-a2i-runtime", "stop-human-loop")]
public record AwsSagemakerA2iRuntimeStopHumanLoopOptions(
[property: CommandSwitch("--human-loop-name")] string HumanLoopName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
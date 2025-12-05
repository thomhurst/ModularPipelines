using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker-a2i-runtime", "start-human-loop")]
public record AwsSagemakerA2iRuntimeStartHumanLoopOptions(
[property: CliOption("--human-loop-name")] string HumanLoopName,
[property: CliOption("--flow-definition-arn")] string FlowDefinitionArn,
[property: CliOption("--human-loop-input")] string HumanLoopInput
) : AwsOptions
{
    [CliOption("--data-attributes")]
    public string? DataAttributes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
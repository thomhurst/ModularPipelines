using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker-a2i-runtime", "describe-human-loop")]
public record AwsSagemakerA2iRuntimeDescribeHumanLoopOptions(
[property: CliOption("--human-loop-name")] string HumanLoopName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
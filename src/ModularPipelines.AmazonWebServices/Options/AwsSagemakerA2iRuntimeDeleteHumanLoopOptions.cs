using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker-a2i-runtime", "delete-human-loop")]
public record AwsSagemakerA2iRuntimeDeleteHumanLoopOptions(
[property: CliOption("--human-loop-name")] string HumanLoopName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
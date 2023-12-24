using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "describe-model-card")]
public record AwsSagemakerDescribeModelCardOptions(
[property: CommandSwitch("--model-card-name")] string ModelCardName
) : AwsOptions
{
    [CommandSwitch("--model-card-version")]
    public int? ModelCardVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
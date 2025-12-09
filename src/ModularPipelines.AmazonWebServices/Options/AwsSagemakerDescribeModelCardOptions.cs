using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "describe-model-card")]
public record AwsSagemakerDescribeModelCardOptions(
[property: CliOption("--model-card-name")] string ModelCardName
) : AwsOptions
{
    [CliOption("--model-card-version")]
    public int? ModelCardVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-model-card")]
public record AwsSagemakerUpdateModelCardOptions(
[property: CliOption("--model-card-name")] string ModelCardName
) : AwsOptions
{
    [CliOption("--content")]
    public string? Content { get; set; }

    [CliOption("--model-card-status")]
    public string? ModelCardStatus { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
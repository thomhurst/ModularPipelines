using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "describe-model-version")]
public record AwsLookoutequipmentDescribeModelVersionOptions(
[property: CliOption("--model-name")] string ModelName,
[property: CliOption("--model-version")] long ModelVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elastic-inference", "describe-accelerator-offerings")]
public record AwsElasticInferenceDescribeAcceleratorOfferingsOptions(
[property: CliOption("--location-type")] string LocationType
) : AwsOptions
{
    [CliOption("--accelerator-types")]
    public string[]? AcceleratorTypes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
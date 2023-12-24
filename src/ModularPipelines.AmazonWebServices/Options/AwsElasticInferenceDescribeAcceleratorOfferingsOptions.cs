using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastic-inference", "describe-accelerator-offerings")]
public record AwsElasticInferenceDescribeAcceleratorOfferingsOptions(
[property: CommandSwitch("--location-type")] string LocationType
) : AwsOptions
{
    [CommandSwitch("--accelerator-types")]
    public string[]? AcceleratorTypes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
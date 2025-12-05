using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "describe-custom-routing-accelerator-attributes")]
public record AwsGlobalacceleratorDescribeCustomRoutingAcceleratorAttributesOptions(
[property: CliOption("--accelerator-arn")] string AcceleratorArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
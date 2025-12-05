using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "describe-fpga-image-attribute")]
public record AwsEc2DescribeFpgaImageAttributeOptions(
[property: CliOption("--fpga-image-id")] string FpgaImageId,
[property: CliOption("--attribute")] string Attribute
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
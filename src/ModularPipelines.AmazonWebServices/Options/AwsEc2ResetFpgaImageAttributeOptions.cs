using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "reset-fpga-image-attribute")]
public record AwsEc2ResetFpgaImageAttributeOptions(
[property: CliOption("--fpga-image-id")] string FpgaImageId
) : AwsOptions
{
    [CliOption("--attribute")]
    public string? Attribute { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "describe-fpga-image-attribute")]
public record AwsEc2DescribeFpgaImageAttributeOptions(
[property: CommandSwitch("--fpga-image-id")] string FpgaImageId,
[property: CommandSwitch("--attribute")] string Attribute
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "reset-fpga-image-attribute")]
public record AwsEc2ResetFpgaImageAttributeOptions(
[property: CommandSwitch("--fpga-image-id")] string FpgaImageId
) : AwsOptions
{
    [CommandSwitch("--attribute")]
    public string? Attribute { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "enable-image-block-public-access")]
public record AwsEc2EnableImageBlockPublicAccessOptions(
[property: CommandSwitch("--image-block-public-access-state")] string ImageBlockPublicAccessState
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
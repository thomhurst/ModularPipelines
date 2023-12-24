using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "enable-image-deprecation")]
public record AwsEc2EnableImageDeprecationOptions(
[property: CommandSwitch("--image-id")] string ImageId,
[property: CommandSwitch("--deprecate-at")] long DeprecateAt
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
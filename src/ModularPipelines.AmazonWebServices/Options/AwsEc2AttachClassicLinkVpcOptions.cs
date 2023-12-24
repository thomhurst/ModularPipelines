using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "attach-classic-link-vpc")]
public record AwsEc2AttachClassicLinkVpcOptions(
[property: CommandSwitch("--groups")] string[] Groups,
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--vpc-id")] string VpcId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
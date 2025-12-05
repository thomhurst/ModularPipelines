using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "attach-classic-link-vpc")]
public record AwsEc2AttachClassicLinkVpcOptions(
[property: CliOption("--groups")] string[] Groups,
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--vpc-id")] string VpcId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-vpc-endpoints")]
public record AwsEc2DeleteVpcEndpointsOptions(
[property: CommandSwitch("--vpc-endpoint-ids")] string[] VpcEndpointIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
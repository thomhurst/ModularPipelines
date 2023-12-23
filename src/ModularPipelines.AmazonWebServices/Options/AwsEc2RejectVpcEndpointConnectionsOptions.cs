using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "reject-vpc-endpoint-connections")]
public record AwsEc2RejectVpcEndpointConnectionsOptions(
[property: CommandSwitch("--service-id")] string ServiceId,
[property: CommandSwitch("--vpc-endpoint-ids")] string[] VpcEndpointIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
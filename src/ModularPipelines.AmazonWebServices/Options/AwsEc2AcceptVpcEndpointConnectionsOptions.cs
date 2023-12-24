using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "accept-vpc-endpoint-connections")]
public record AwsEc2AcceptVpcEndpointConnectionsOptions(
[property: CommandSwitch("--service-id")] string ServiceId,
[property: CommandSwitch("--vpc-endpoint-ids")] string[] VpcEndpointIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
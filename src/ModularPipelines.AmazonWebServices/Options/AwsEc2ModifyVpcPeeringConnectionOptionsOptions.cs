using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-vpc-peering-connection-options")]
public record AwsEc2ModifyVpcPeeringConnectionOptionsOptions(
[property: CommandSwitch("--vpc-peering-connection-id")] string VpcPeeringConnectionId
) : AwsOptions
{
    [CommandSwitch("--accepter-peering-connection-options")]
    public string? AccepterPeeringConnectionOptions { get; set; }

    [CommandSwitch("--requester-peering-connection-options")]
    public string? RequesterPeeringConnectionOptions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
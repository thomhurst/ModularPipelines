using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3outposts", "create-endpoint")]
public record AwsS3outpostsCreateEndpointOptions(
[property: CommandSwitch("--outpost-id")] string OutpostId,
[property: CommandSwitch("--subnet-id")] string SubnetId,
[property: CommandSwitch("--security-group-id")] string SecurityGroupId
) : AwsOptions
{
    [CommandSwitch("--access-type")]
    public string? AccessType { get; set; }

    [CommandSwitch("--customer-owned-ipv4-pool")]
    public string? CustomerOwnedIpv4Pool { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
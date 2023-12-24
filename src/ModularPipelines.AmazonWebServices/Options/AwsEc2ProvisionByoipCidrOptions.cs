using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "provision-byoip-cidr")]
public record AwsEc2ProvisionByoipCidrOptions(
[property: CommandSwitch("--cidr")] string Cidr
) : AwsOptions
{
    [CommandSwitch("--cidr-authorization-context")]
    public string? CidrAuthorizationContext { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--pool-tag-specifications")]
    public string[]? PoolTagSpecifications { get; set; }

    [CommandSwitch("--network-border-group")]
    public string? NetworkBorderGroup { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
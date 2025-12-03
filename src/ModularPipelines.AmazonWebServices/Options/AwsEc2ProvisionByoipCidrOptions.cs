using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "provision-byoip-cidr")]
public record AwsEc2ProvisionByoipCidrOptions(
[property: CliOption("--cidr")] string Cidr
) : AwsOptions
{
    [CliOption("--cidr-authorization-context")]
    public string? CidrAuthorizationContext { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--pool-tag-specifications")]
    public string[]? PoolTagSpecifications { get; set; }

    [CliOption("--network-border-group")]
    public string? NetworkBorderGroup { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
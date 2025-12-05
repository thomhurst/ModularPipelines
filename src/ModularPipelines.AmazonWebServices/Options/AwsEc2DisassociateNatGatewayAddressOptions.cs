using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "disassociate-nat-gateway-address")]
public record AwsEc2DisassociateNatGatewayAddressOptions(
[property: CliOption("--nat-gateway-id")] string NatGatewayId,
[property: CliOption("--association-ids")] string[] AssociationIds
) : AwsOptions
{
    [CliOption("--max-drain-duration-seconds")]
    public int? MaxDrainDurationSeconds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
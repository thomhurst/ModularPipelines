using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "disassociate-address")]
public record AwsEc2DisassociateAddressOptions : AwsOptions
{
    [CliOption("--association-id")]
    public string? AssociationId { get; set; }

    [CliOption("--public-ip")]
    public string? PublicIp { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
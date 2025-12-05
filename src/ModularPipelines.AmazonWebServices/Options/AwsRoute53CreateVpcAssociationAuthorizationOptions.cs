using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "create-vpc-association-authorization")]
public record AwsRoute53CreateVpcAssociationAuthorizationOptions(
[property: CliOption("--hosted-zone-id")] string HostedZoneId,
[property: CliOption("--vpc")] string Vpc
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
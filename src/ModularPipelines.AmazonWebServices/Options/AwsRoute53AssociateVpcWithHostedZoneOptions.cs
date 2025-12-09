using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "associate-vpc-with-hosted-zone")]
public record AwsRoute53AssociateVpcWithHostedZoneOptions(
[property: CliOption("--hosted-zone-id")] string HostedZoneId,
[property: CliOption("--vpc")] string Vpc
) : AwsOptions
{
    [CliOption("--comment")]
    public string? Comment { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
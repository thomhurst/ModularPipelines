using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "list-vpc-association-authorizations")]
public record AwsRoute53ListVpcAssociationAuthorizationsOptions(
[property: CliOption("--hosted-zone-id")] string HostedZoneId
) : AwsOptions
{
    [CliOption("--max-results")]
    public string? MaxResults { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
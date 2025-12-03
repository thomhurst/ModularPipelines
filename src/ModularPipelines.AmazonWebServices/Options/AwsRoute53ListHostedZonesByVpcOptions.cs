using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "list-hosted-zones-by-vpc")]
public record AwsRoute53ListHostedZonesByVpcOptions(
[property: CliOption("--vpc-id")] string VpcId,
[property: CliOption("--vpc-region")] string VpcRegion
) : AwsOptions
{
    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
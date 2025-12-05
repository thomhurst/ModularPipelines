using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearchserverless", "list-vpc-endpoints")]
public record AwsOpensearchserverlessListVpcEndpointsOptions : AwsOptions
{
    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--vpc-endpoint-filters")]
    public string? VpcEndpointFilters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
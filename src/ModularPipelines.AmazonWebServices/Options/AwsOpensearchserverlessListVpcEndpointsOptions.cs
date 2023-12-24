using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearchserverless", "list-vpc-endpoints")]
public record AwsOpensearchserverlessListVpcEndpointsOptions : AwsOptions
{
    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--vpc-endpoint-filters")]
    public string? VpcEndpointFilters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
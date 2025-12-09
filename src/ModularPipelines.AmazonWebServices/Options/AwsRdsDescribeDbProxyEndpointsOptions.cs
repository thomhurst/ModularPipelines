using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "describe-db-proxy-endpoints")]
public record AwsRdsDescribeDbProxyEndpointsOptions : AwsOptions
{
    [CliOption("--db-proxy-name")]
    public string? DbProxyName { get; set; }

    [CliOption("--db-proxy-endpoint-name")]
    public string? DbProxyEndpointName { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "describe-verified-access-endpoints")]
public record AwsEc2DescribeVerifiedAccessEndpointsOptions : AwsOptions
{
    [CliOption("--verified-access-endpoint-ids")]
    public string[]? VerifiedAccessEndpointIds { get; set; }

    [CliOption("--verified-access-instance-id")]
    public string? VerifiedAccessInstanceId { get; set; }

    [CliOption("--verified-access-group-id")]
    public string? VerifiedAccessGroupId { get; set; }

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
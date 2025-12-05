using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "describe-application-fleet-associations")]
public record AwsAppstreamDescribeApplicationFleetAssociationsOptions : AwsOptions
{
    [CliOption("--fleet-name")]
    public string? FleetName { get; set; }

    [CliOption("--application-arn")]
    public string? ApplicationArn { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
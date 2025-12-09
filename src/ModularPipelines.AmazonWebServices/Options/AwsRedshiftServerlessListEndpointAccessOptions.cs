using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "list-endpoint-access")]
public record AwsRedshiftServerlessListEndpointAccessOptions : AwsOptions
{
    [CliOption("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CliOption("--vpc-id")]
    public string? VpcId { get; set; }

    [CliOption("--workgroup-name")]
    public string? WorkgroupName { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
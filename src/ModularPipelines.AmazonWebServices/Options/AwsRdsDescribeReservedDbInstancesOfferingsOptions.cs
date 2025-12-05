using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "describe-reserved-db-instances-offerings")]
public record AwsRdsDescribeReservedDbInstancesOfferingsOptions : AwsOptions
{
    [CliOption("--reserved-db-instances-offering-id")]
    public string? ReservedDbInstancesOfferingId { get; set; }

    [CliOption("--db-instance-class")]
    public string? DbInstanceClass { get; set; }

    [CliOption("--duration")]
    public string? Duration { get; set; }

    [CliOption("--product-description")]
    public string? ProductDescription { get; set; }

    [CliOption("--offering-type")]
    public string? OfferingType { get; set; }

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
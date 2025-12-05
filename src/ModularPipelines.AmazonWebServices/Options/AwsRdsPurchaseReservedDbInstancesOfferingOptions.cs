using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "purchase-reserved-db-instances-offering")]
public record AwsRdsPurchaseReservedDbInstancesOfferingOptions(
[property: CliOption("--reserved-db-instances-offering-id")] string ReservedDbInstancesOfferingId
) : AwsOptions
{
    [CliOption("--reserved-db-instance-id")]
    public string? ReservedDbInstanceId { get; set; }

    [CliOption("--db-instance-count")]
    public int? DbInstanceCount { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
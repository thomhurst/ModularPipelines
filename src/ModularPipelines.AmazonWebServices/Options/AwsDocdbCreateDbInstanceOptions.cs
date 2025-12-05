using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("docdb", "create-db-instance")]
public record AwsDocdbCreateDbInstanceOptions(
[property: CliOption("--db-instance-identifier")] string DbInstanceIdentifier,
[property: CliOption("--db-instance-class")] string DbInstanceClass,
[property: CliOption("--engine")] string Engine,
[property: CliOption("--db-cluster-identifier")] string DbClusterIdentifier
) : AwsOptions
{
    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--promotion-tier")]
    public int? PromotionTier { get; set; }

    [CliOption("--performance-insights-kms-key-id")]
    public string? PerformanceInsightsKmsKeyId { get; set; }

    [CliOption("--ca-certificate-identifier")]
    public string? CaCertificateIdentifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
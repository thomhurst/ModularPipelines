using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("docdb", "modify-db-instance")]
public record AwsDocdbModifyDbInstanceOptions(
[property: CliOption("--db-instance-identifier")] string DbInstanceIdentifier
) : AwsOptions
{
    [CliOption("--db-instance-class")]
    public string? DbInstanceClass { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--new-db-instance-identifier")]
    public string? NewDbInstanceIdentifier { get; set; }

    [CliOption("--ca-certificate-identifier")]
    public string? CaCertificateIdentifier { get; set; }

    [CliOption("--promotion-tier")]
    public int? PromotionTier { get; set; }

    [CliOption("--performance-insights-kms-key-id")]
    public string? PerformanceInsightsKmsKeyId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
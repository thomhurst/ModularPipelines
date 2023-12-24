using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("docdb", "create-db-instance")]
public record AwsDocdbCreateDbInstanceOptions(
[property: CommandSwitch("--db-instance-identifier")] string DbInstanceIdentifier,
[property: CommandSwitch("--db-instance-class")] string DbInstanceClass,
[property: CommandSwitch("--engine")] string Engine,
[property: CommandSwitch("--db-cluster-identifier")] string DbClusterIdentifier
) : AwsOptions
{
    [CommandSwitch("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CommandSwitch("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--promotion-tier")]
    public int? PromotionTier { get; set; }

    [CommandSwitch("--performance-insights-kms-key-id")]
    public string? PerformanceInsightsKmsKeyId { get; set; }

    [CommandSwitch("--ca-certificate-identifier")]
    public string? CaCertificateIdentifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "record-sets", "update")]
public record GcloudDnsRecordSetsUpdateOptions(
[property: CliArgument] string DnsName,
[property: CliOption("--type")] string Type,
[property: CliOption("--zone")] string Zone,
[property: CliOption("--rrdatas")] string[] Rrdatas,
[property: CliOption("--routing-policy-type")] string RoutingPolicyType,
[property: CliFlag("--enable-geo-fencing")] bool EnableGeoFencing,
[property: CliFlag("--enable-health-checking")] bool EnableHealthChecking,
[property: CliOption("--routing-policy-data")] string RoutingPolicyData,
[property: CliOption("--routing-policy-backup-data")] string RoutingPolicyBackupData,
[property: CliOption("--routing-policy-backup-data-type")] string RoutingPolicyBackupDataType,
[property: CliOption("--routing-policy-primary-data")] string RoutingPolicyPrimaryData,
[property: CliOption("--backup-data-trickle-ratio")] string BackupDataTrickleRatio
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--ttl")]
    public string? Ttl { get; set; }
}
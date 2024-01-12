using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "record-sets", "update")]
public record GcloudDnsRecordSetsUpdateOptions(
[property: PositionalArgument] string DnsName,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--zone")] string Zone,
[property: CommandSwitch("--rrdatas")] string[] Rrdatas,
[property: CommandSwitch("--routing-policy-type")] string RoutingPolicyType,
[property: BooleanCommandSwitch("--enable-geo-fencing")] bool EnableGeoFencing,
[property: BooleanCommandSwitch("--enable-health-checking")] bool EnableHealthChecking,
[property: CommandSwitch("--routing-policy-data")] string RoutingPolicyData,
[property: CommandSwitch("--routing-policy-backup-data")] string RoutingPolicyBackupData,
[property: CommandSwitch("--routing-policy-backup-data-type")] string RoutingPolicyBackupDataType,
[property: CommandSwitch("--routing-policy-primary-data")] string RoutingPolicyPrimaryData,
[property: CommandSwitch("--backup-data-trickle-ratio")] string BackupDataTrickleRatio
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--ttl")]
    public string? Ttl { get; set; }
}
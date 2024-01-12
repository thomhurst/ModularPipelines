using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("metastore", "services", "create")]
public record GcloudMetastoreServicesCreateOptions(
[property: PositionalArgument] string Service,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--data-catalog-sync")]
    public bool? DataCatalogSync { get; set; }

    [CommandSwitch("--database-type")]
    public string? DatabaseType { get; set; }

    [CommandSwitch("--encryption-kms-key")]
    public string? EncryptionKmsKey { get; set; }

    [CommandSwitch("--endpoint-protocol")]
    public string? EndpointProtocol { get; set; }

    [CommandSwitch("--hive-metastore-version")]
    public string? HiveMetastoreVersion { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--port")]
    public string? Port { get; set; }

    [CommandSwitch("--release-channel")]
    public string? ReleaseChannel { get; set; }

    [CommandSwitch("--auxiliary-versions")]
    public string[]? AuxiliaryVersions { get; set; }

    [CommandSwitch("--auxiliary-versions-from-file")]
    public string? AuxiliaryVersionsFromFile { get; set; }

    [CommandSwitch("--consumer-subnetworks")]
    public string[]? ConsumerSubnetworks { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--network-config-from-file")]
    public string? NetworkConfigFromFile { get; set; }

    [CommandSwitch("--hive-metastore-configs")]
    public IEnumerable<KeyValue>? HiveMetastoreConfigs { get; set; }

    [CommandSwitch("--hive-metastore-configs-from-file")]
    public string? HiveMetastoreConfigsFromFile { get; set; }

    [CommandSwitch("--instance-size")]
    public string? InstanceSize { get; set; }

    [BooleanCommandSwitch("extra-large")]
    public bool? ExtraLarge { get; set; }

    [BooleanCommandSwitch("extra-small")]
    public bool? ExtraSmall { get; set; }

    [BooleanCommandSwitch("large")]
    public bool? Large { get; set; }

    [BooleanCommandSwitch("medium")]
    public bool? Medium { get; set; }

    [BooleanCommandSwitch("small")]
    public bool? Small { get; set; }

    [CommandSwitch("--scaling-factor")]
    public string? ScalingFactor { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [BooleanCommandSwitch("developer")]
    public bool? Developer { get; set; }

    [BooleanCommandSwitch("enterprise")]
    public bool? Enterprise { get; set; }

    [CommandSwitch("--kerberos-principal")]
    public string? KerberosPrincipal { get; set; }

    [CommandSwitch("--keytab")]
    public string? Keytab { get; set; }

    [CommandSwitch("--krb5-config")]
    public string? Krb5Config { get; set; }

    [CommandSwitch("--maintenance-window-day")]
    public string? MaintenanceWindowDay { get; set; }

    [CommandSwitch("--maintenance-window-hour")]
    public string? MaintenanceWindowHour { get; set; }
}
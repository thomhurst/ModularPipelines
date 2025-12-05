using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "services", "create")]
public record GcloudMetastoreServicesCreateOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--data-catalog-sync")]
    public bool? DataCatalogSync { get; set; }

    [CliOption("--database-type")]
    public string? DatabaseType { get; set; }

    [CliOption("--encryption-kms-key")]
    public string? EncryptionKmsKey { get; set; }

    [CliOption("--endpoint-protocol")]
    public string? EndpointProtocol { get; set; }

    [CliOption("--hive-metastore-version")]
    public string? HiveMetastoreVersion { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }

    [CliOption("--release-channel")]
    public string? ReleaseChannel { get; set; }

    [CliOption("--auxiliary-versions")]
    public string[]? AuxiliaryVersions { get; set; }

    [CliOption("--auxiliary-versions-from-file")]
    public string? AuxiliaryVersionsFromFile { get; set; }

    [CliOption("--consumer-subnetworks")]
    public string[]? ConsumerSubnetworks { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--network-config-from-file")]
    public string? NetworkConfigFromFile { get; set; }

    [CliOption("--hive-metastore-configs")]
    public IEnumerable<KeyValue>? HiveMetastoreConfigs { get; set; }

    [CliOption("--hive-metastore-configs-from-file")]
    public string? HiveMetastoreConfigsFromFile { get; set; }

    [CliOption("--instance-size")]
    public string? InstanceSize { get; set; }

    [CliFlag("extra-large")]
    public bool? ExtraLarge { get; set; }

    [CliFlag("extra-small")]
    public bool? ExtraSmall { get; set; }

    [CliFlag("large")]
    public bool? Large { get; set; }

    [CliFlag("medium")]
    public bool? Medium { get; set; }

    [CliFlag("small")]
    public bool? Small { get; set; }

    [CliOption("--scaling-factor")]
    public string? ScalingFactor { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliFlag("developer")]
    public bool? Developer { get; set; }

    [CliFlag("enterprise")]
    public bool? Enterprise { get; set; }

    [CliOption("--kerberos-principal")]
    public string? KerberosPrincipal { get; set; }

    [CliOption("--keytab")]
    public string? Keytab { get; set; }

    [CliOption("--krb5-config")]
    public string? Krb5Config { get; set; }

    [CliOption("--maintenance-window-day")]
    public string? MaintenanceWindowDay { get; set; }

    [CliOption("--maintenance-window-hour")]
    public string? MaintenanceWindowHour { get; set; }
}